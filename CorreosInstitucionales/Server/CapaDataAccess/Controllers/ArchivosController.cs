﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Text;

using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Server.Utils;

using CorreosInstitucionales.Shared;
using CorreosInstitucionales.Shared.CapaEntities.Common;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendNotificaciones;
using CorreosInstitucionales.Client.CapaPresentationComponentsPagesUI_UX.MóduloCatálogos;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ArchivosController (
            DbCorreosInstitucionalesUpiicsaContext db,
            RSendNotificacionesService servicioNotificaciones
        ) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly RSendNotificacionesService _servicioNotificaciones = servicioNotificaciones;

        [ApiExplorerSettings(IgnoreApi = true)]
        protected string? FormatoTexto(string? texto)
        {
            if (texto is null) return null;

            Dictionary<char, char> pairs = new()
            {
                { 'Á' , 'A'},
                { 'É' , 'E'},
                { 'Í' , 'I'},
                { 'Ó' , 'O'},
                { 'Ú' , 'U'},
                { 'Ñ' , 'N'},
            };

            string tmp = texto;

            foreach(KeyValuePair<char,char> lookup in pairs)
            {
                tmp = tmp.Replace(lookup.Key, lookup.Value);
            }

            return tmp;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected string? EnlaceRoto(string? archivo, string ruta)
        {
            if (archivo is null || archivo == "-") return null;

            string basedir = ServerFS.GetBaseDir(true);

            string origen = $"{basedir}/assets/cheems.pdf";
            string destino = Path.GetFullPath($"{ServerFS.Root}/{ruta}{archivo}");
            string? dir = Path.GetDirectoryName(destino);
            bool enlace_roto = !System.IO.File.Exists(destino);

            if (enlace_roto)
            {
                Directory.CreateDirectory(dir!);
                System.IO.File.Copy(origen, destino, false );
            }

            return enlace_roto? ruta+archivo : null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected async Task<string?> EstablecerEstado(IEnumerable<MtTbSolicitudesTicket> lista_solicitudes, int estado)
        {
            string? response = null;
            int[] pendientes = lista_solicitudes.Select(st => st.IdSolicitudTicket).ToArray();
            
            try
            {
                List<MtTbSolicitudesTicket> solicitudes = await _db.MtTbSolicitudesTickets
                    .Where(ts => pendientes.Contains(ts.IdSolicitudTicket))
                    .ToListAsync();

                solicitudes.ForEach(st => st.SolIdEstadoSolicitud = estado);

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<string> EnvioMasivo(
            IEnumerable<MtTbSolicitudesTicket> lista,
            int tipoEstado
        )
        {
            return await _servicioNotificaciones.EnvioMasivoAsync(lista, tipoEstado);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private WebUtils.Link GenerarLink(
            MtTbSolicitudesTicket solicitud, 
            TipoDocumento tipo_documento, 
            string directorio, 
            bool optional = true)
        {
            string archivo = string.Empty;
            string tipo = tipo_documento.GetNombre();

            string curp = solicitud.SolIdUsuarioNavigation!.UsuCurp;
            string id = string.Format("{0:00000}", solicitud.IdSolicitudTicket);

            MpTbUsuario usuario = solicitud.SolIdUsuarioNavigation;

            switch (tipo_documento)
            {
                case TipoDocumento.CURP:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameCurp!;
                    break;
                case TipoDocumento.COM_INSCRIPCION:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameComprobanteEstudios!;
                    break;
                case TipoDocumento.CAP_ANTIVIRUS:
                    archivo = solicitud.SolCapturaEscaneoAntivirus!;
                    break;
                case TipoDocumento.CAP_BLOQUEO:
                    archivo = solicitud.SolCapturaCuentaBloqueada!;
                    break;
                case TipoDocumento.CAP_ERROR:
                    archivo = solicitud.SolCapturaError!;
                    break;
            }

            string ext = Path.GetExtension(archivo);
            string nombre_usuario = $"{usuario.UsuNombres} {usuario.UsuPrimerApellido} {usuario.UsuSegundoApellido}";
            return new WebUtils.Link($"{directorio}{archivo}",$"{nombre_usuario}-{tipo}{ext}", optional);
        }

        [HttpPost("xlsx/pendientes")]
        public async Task<IActionResult> ExportarPendientes_XLSX(TExport<int[]>data)
        {
            return await LlenarFormulario(data, false);
        }

        [HttpPost("zip/pendientes")]
        public async Task<IActionResult> ExportarPendientes_ZIP(TExport<int[]> data)
        {
            return await LlenarFormulario(data, true);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public TipoDatoXLSX GetTipoDato(string columna)
        {
            string colname = columna.Replace("Á", "A").Trim();
            colname = colname.Replace("É", "E");
            colname = colname.Replace("Í", "I");
            colname = colname.Replace("Ó", "O");
            colname = colname.Replace("Ú", "U");

            switch (colname)
            {
                case "CORREO PERSONAL ACTUAL":  return TipoDatoXLSX.CORREO_PERSONAL;
                case "CORREO PERSONAL":         return TipoDatoXLSX.CORREO_PERSONAL;

                case "CORREO INSTITUCIONAL": return TipoDatoXLSX.CORREO_INSTITUCIONAL;
                case "CORREO INSTITUCIONAL ACTUAL": return TipoDatoXLSX.CORREO_INSTITUCIONAL;
                case "CONTRASEÑA": return TipoDatoXLSX.CONTRA;

                case "ACCION": return TipoDatoXLSX.ACCION;

                case "NUMERO DE CELULAR ACTUAL": return TipoDatoXLSX.CELULAR;
                case "CELULAR ACTUAL": return TipoDatoXLSX.CELULAR;
                case "CELULAR": return TipoDatoXLSX.CELULAR;

                case "EXTENSION": return TipoDatoXLSX.EXTENSION;
                case "EXTENSION ACTUAL": return TipoDatoXLSX.EXTENSION;
                case "EXTENSION (SOLO EMPLEADOS DE CONTAR CON ELLA)": return TipoDatoXLSX.EXTENSION;

                case "AREA": return TipoDatoXLSX.ID_EXTERNO;
                case "AREA ACTUAL": return TipoDatoXLSX.ID_EXTERNO;

                case "CLAVE CURP": return TipoDatoXLSX.CURP;
                case "CURP": return TipoDatoXLSX.CURP;

                case "NUMERO DE EMPLEADO O NUMERO DE BOLETA SEGUN SEA EL CASO": return TipoDatoXLSX.ID_EXTERNO;
            }

            return TipoDatoXLSX.NINGUNO;
        }

        [HttpPost("subir/anuncio")]
        public async Task<IActionResult> SubirAnuncio(
            IFormFile file)
        {
            Response<string> oResponse = new();

            string basedir = ServerFS.GetBaseDir(true) + "/banners/";
            string fname = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            try
            {
                using (FileStream fs = new FileStream(basedir + fname, FileMode.CreateNew))
                {
                    await file.CopyToAsync(fs);
                }

                oResponse.Success = 1;
                oResponse.Data = fname;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
                oResponse.Data = ex.StackTrace;
            }

            return Ok(oResponse);
        }


        [HttpPost("xlsx/procesados")]
        public async Task<IActionResult> ImportarProcesados_XLSX(
            IFormFile file)
        {
            Response<string> oResponse = new();

            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string basedir = ServerFS.GetBaseDir(true);
            string filename = $"{basedir}/repositorio/procesados/{id_fecha}_solicitud_alta_desbloqueo_{id}";
            
            List<string> logs = [];
            
            Dictionary<string,RegistroImportacion> registros = [];
            RegistroImportacion registro_actual;

            List<string> lista_curps = [];

            string CURP = string.Empty;

            TipoDatoXLSX[] datos_a_actualizar = [];
            // bool actualizar_todo = false;

            Dictionary<TipoDatoXLSX, int> columna = new()
            {
                {TipoDatoXLSX.CURP,                 100 },
                {TipoDatoXLSX.ID_EXTERNO,           100 },
                {TipoDatoXLSX.AREA,                 100 },
                {TipoDatoXLSX.EXTENSION,            100 },
                {TipoDatoXLSX.CELULAR,              100 },
                {TipoDatoXLSX.CORREO_PERSONAL,      100 },
                {TipoDatoXLSX.CORREO_INSTITUCIONAL, 100 },
                {TipoDatoXLSX.CONTRA,               100 },
                {TipoDatoXLSX.ACCION,               100 },
            };

            TipoDatoXLSX tipo_dato = TipoDatoXLSX.NINGUNO;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    XLWorkbook wb = new XLWorkbook(stream);
                    IXLWorksheet ws = wb.Worksheet(1);

                    int n = ws.LastRowUsed().RowNumber();
                    
                    if (n > 4)
                    {
                        int col = ws.Row(4).LastCellUsed().Address.ColumnNumber;
                        string colname = string.Empty;

                        for(int i = 1; i<= col; i++)
                        {
                            colname = ws.Cell(4, i).Value.ToString().ToUpper();

                            tipo_dato = GetTipoDato(colname);

                            if(tipo_dato != TipoDatoXLSX.NINGUNO)
                            {
                                columna[tipo_dato] = i;
                            }
                        }

                        for (int row = 5; row<=n; row++)
                        {
                            CURP = ws.Cell(row, columna[TipoDatoXLSX.CURP]).Value.ToString().Trim();

                            if (string.IsNullOrEmpty(CURP ))
                            {
                                IXLAddress tmp = ws.Cell(row, columna[TipoDatoXLSX.CURP]).Address;
                                logs.Add($"[INFO] CURP VACÍO EN {tmp.ToString()}, IGNORANDO");
                                continue;
                            }
                            
                            lista_curps.Add(CURP);

                            registro_actual = new RegistroImportacion()
                            {
                                CURP = CURP,
                                ID = ws.Cell(row, columna[TipoDatoXLSX.ID_EXTERNO]).Value.ToString(),
                                NoExtension = ws.Cell(row, columna[TipoDatoXLSX.EXTENSION]).Value.ToString(),
                                Celular = ws.Cell(row, columna[TipoDatoXLSX.CELULAR]).Value.ToString(),
                                CorreoPersonal = ws.Cell(row, columna[TipoDatoXLSX.CORREO_PERSONAL]).Value.ToString(),
                                CorreoInstitucional = ws.Cell(row, columna[TipoDatoXLSX.CORREO_INSTITUCIONAL]).Value.ToString(),
                                Clave = ws.Cell(row, columna[TipoDatoXLSX.CONTRA]).Value.ToString(),
                                Accion = ws.Cell(row, columna[TipoDatoXLSX.ACCION]).Value.ToString()
                            };

                            if(string.IsNullOrEmpty(registro_actual.Accion))
                            {
                                registro_actual.Accion = "Registro actualizado";
                            }

                            registros.Add(CURP, registro_actual);
                        }

                        logs.Add("[INFO] SE ENCONTRARON LAS SIGUIENTES COLUMNAS:");

                        IXLCell cell;

                        foreach (KeyValuePair<TipoDatoXLSX, int> dato in columna)
                        {
                            if(dato.Value == 100)
                            {
                                continue;
                            }

                            cell = ws.Cell(4, dato.Value);
                            logs.Add($"\t - {cell.Address} : {dato.Key.GetNombre()}");
                        }

                    }//LEER XLSX
                    else
                    {
                        logs.Add("[INFO] EL ARCHIVO NO CUENTA CON REGISTROS.");
                    }

                    List<MtTbSolicitudesTicket> notificar = new();

                    List<MtTbSolicitudesTicket> solicitudes = await _db.MtTbSolicitudesTickets
                        .Where
                        (
                            st =>
                                st.SolIdEstadoSolicitud == ((int)TipoEstadoSolicitud.EN_PROCESO) &&
                                lista_curps.Contains(st.SolIdUsuarioNavigation!.UsuCurp!)
                        )
                        .Include(st => st.SolIdUsuarioNavigation)
                        .ToListAsync();

                    if (solicitudes.Count > 0)
                    {
                        logs.Add($"[INFO] {Environment.NewLine}SE ACTUALIZARÁN {solicitudes.Count} PENDIENTE{(solicitudes.Count == 1 ? string.Empty : "S")} DE {registros.Count} REGISTRO{(registros.Count == 1 ? string.Empty : "S")} DEL ARCHIVO...");
                    }
                    else
                    {
                        logs.Add($"[INFO] NO SE ACTUALIZARÁN REGISTROS...");
                    }

                    logs.Add(RegistroImportacion.GetHeaders());

                    bool actualizar_todo = false;

                    solicitudes.ForEach(solicitud =>
                    {
                        registro_actual = registros[solicitud.SolIdUsuarioNavigation.UsuCurp];
                        registro_actual.Ticket = solicitud.IdSolicitudTicket.ToString();

                        datos_a_actualizar = ((TipoSolicitud)solicitud.SolIdTipoSolicitud).GetDatosActualizar();
                        actualizar_todo = datos_a_actualizar.Contains(TipoDatoXLSX.TODO);
                        
                        List<string> columnas_actualizar = [];

                        foreach(TipoDatoXLSX columna in datos_a_actualizar)
                        {
                            columnas_actualizar.Add(columna.GetNombre());
                        }

                        string s_columnas_actualizar = string.Join("|", columnas_actualizar);

                        logs.Add($"{registro_actual} -> {s_columnas_actualizar}");

                        if (datos_a_actualizar.Contains(TipoDatoXLSX.NINGUNO))
                        {
                            logs.Add($"\t - EL TIPO DE SOLICITUD NO REQUIRE ACTUALIZAR DATOS(?)");
                            return;
                        }
                        
                        if (datos_a_actualizar.Contains(TipoDatoXLSX.CORREO_INSTITUCIONAL) || actualizar_todo)
                        {
                            if (string.IsNullOrEmpty(registro_actual.CorreoInstitucional))
                            {
                                logs.Add($"\t - [ADVERTENCIA] {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE CORREO INSTITUCIONAL NO ESTÉ VACÍA.");

                                if ((TipoSolicitud)solicitud.SolIdTipoSolicitud != TipoSolicitud.OTRO)
                                {
                                    logs.Add("\t - [ERROR] ÚNICAMENTE SE ADMITEN VALORES VACIOS EN SOLICIUDES DEL TIPO 'OTRO'. IGNORANDO CAMBIOS");
                                    return;
                                }
                            }
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta = registro_actual.CorreoInstitucional;
                            logs.Add($"\t - [INFO] SE ACTUALIZÓ EL CORREO INSTITUCIONAL");
                        }

                        if (datos_a_actualizar.Contains(TipoDatoXLSX.CONTRA) || actualizar_todo)
                        {
                            if (string.IsNullOrEmpty(registro_actual.Clave))
                            {
                                logs.Add($"\t - [ADVERTENCIA] {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE CONTRASEÑA NO ESTÉ VACÍA.");

                                if ((TipoSolicitud)solicitud.SolIdTipoSolicitud != TipoSolicitud.OTRO)
                                {
                                    logs.Add("\t - [ERROR] ÚNICAMENTE SE ADMITEN VALORES VACIOS EN SOLICIUDES DEL TIPO 'OTRO'. IGNORANDO CAMBIOS");
                                    return;
                                }
                            }
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContrasenia = registro_actual.Clave;
                            logs.Add($"\t - [INFO] SE ACTUALIZÓ LA CONTRASEÑA");
                        }

                        solicitud.SolRespuestaDcyC = registro_actual.Accion;
                        solicitud.SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.ATENDIDA;
                        solicitud.SolFechaHoraActualizacion = DateTime.Now;

                        notificar.Add(solicitud);
                    });

                    // GUARDAR ÚNICAMENTE SI ES NECESARIO
                    if (solicitudes.Count > 0)
                    {
                        int guardados = await _db.SaveChangesAsync();

                        if (guardados > 0)
                        {
                            await EnvioMasivo(notificar, (int)TipoEstadoSolicitud.ATENDIDA);
                        }
                    }
                }

                oResponse.Success = 1;
            }
            catch(Exception ex)
            {
                oResponse.Message = ex.Message;
                logs.Add($"{ex.Message}:{Environment.NewLine}{ex.StackTrace}");
            }

            string log_content = string.Join(Environment.NewLine, logs);

            await System.IO.File.WriteAllTextAsync($"{filename}.log", log_content);

            oResponse.Data = log_content;

            return Ok(oResponse);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<WebUtils.Link> GenerarXLSX(IEnumerable<MtTbSolicitudesTicket> lista, string ruta, TipoSolicitud formato_solicitud = TipoSolicitud.NO_ESPECIFICADA)
        {
            List<WebUtils.Link> links = [];

            // ARCHIVOS
            string plantilla = $"{ServerFS.GetBaseDir(true)}/assets/{formato_solicitud.GetPlantilla()}";

            string archivo_exportacion = ruta.Replace("%ruta%",formato_solicitud.GetNombreExportacion())+".xlsx";
            string nombre_archivo = Path.GetFileName(archivo_exportacion);
            
            string ruta_usuario = string.Empty;
            string ruta_repositorio = string.Empty;

            // EXCEL
            XLWorkbook wb = new XLWorkbook(plantilla);
            IXLWorksheet ws = wb.Worksheet(1);
            ws.Cell("H2").Value = DateTime.Now.ToString("dd/MM/yyyy");//FECHA

            // DATOS USUARIO
            MpTbUsuario usuario;
            TipoPersonal tipo_personal;

            // DATOS SOLICITUD
            string? id_externo_usuario = null;

            List<TipoDatoXLSX> datos_exportar = new();// formato_solicitud.GetDatosExportar();

            // OBTENEMOS TODOS LOS DISTINTOS TIPOS DE SOLIICTUD
            TipoSolicitud[] tipos_solicitud = lista.
                GroupBy(s => s.SolIdTipoSolicitud).
                Select(g => g.First()).
                Select(s=>s.SolIdTipoSolicitud).
                Cast<TipoSolicitud>().
                ToArray();

            foreach(TipoSolicitud tipo in tipos_solicitud)
            {
                datos_exportar.AddRange(tipo.GetDatosExportar());
            }

            datos_exportar = datos_exportar.Distinct().ToList();

            TipoDocumento[] documentos_adjuntar;
            TipoSolicitud tipo_solicitud;

            int fila = 5; //PRIMER FILA A LEER
            int columna = 5;//PRIMER COLUMNA DISPONIBLE
            int columna_final = 5;
            
            foreach(TipoDatoXLSX dato in datos_exportar)
            {
                ws.Cell(4, columna_final).Value = FormatoTexto(dato.GetNombre().ToUpper());
                columna_final++;
            }
            columna_final--;

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                ruta_repositorio = $"repositorio/solicitudes-tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                ruta_usuario = $"repositorio/usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

                usuario = solicitud.SolIdUsuarioNavigation!;

                id_externo_usuario = usuario.UsuNumeroEmpleadoContrato;
                
                tipo_personal = (TipoPersonal)usuario.UsuIdTipoPersonal!;
                tipo_solicitud = (TipoSolicitud)solicitud.SolIdTipoSolicitud!;

                switch (tipo_personal)
                {
                    case TipoPersonal.ALUMNO:
                    case TipoPersonal.EGRESADO:
                        id_externo_usuario = usuario.UsuBoletaAlumnoEgresado;
                        break;
                    case TipoPersonal.POSGRADO:
                        id_externo_usuario = usuario.UsuBoletaPosgrado;
                        break;
                }

                ws.Cell(fila, 1).Value = FormatoTexto(usuario.UsuNombres);
                ws.Cell(fila, 2).Value = FormatoTexto(usuario.UsuPrimerApellido);
                ws.Cell(fila, 3).Value = FormatoTexto(usuario.UsuSegundoApellido);
                ws.Cell(fila, 4).Value = ((TipoPersonal)usuario.UsuIdTipoPersonal).GetNombre();

                foreach(TipoDatoXLSX dato in datos_exportar)
                {
                    columna = 5 + datos_exportar.IndexOf(dato);

                    switch(dato)
                    {
                        case TipoDatoXLSX.CURP:
                            ws.Cell(fila, columna).Value = usuario.UsuCurp;
                            break;

                        case TipoDatoXLSX.CORREO_PERSONAL: 
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoPersonalCuentaAnterior; 
                            break;
                        case TipoDatoXLSX.CORREO_PERSONAL_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoPersonalCuentaActual;
                            break;

                        case TipoDatoXLSX.CORREO_INSTITUCIONAL:
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoInstitucionalCuenta;

                            break;
                        case TipoDatoXLSX.CELULAR:
                            ws.Cell(fila, columna).Value = usuario.UsuNoCelularAnterior;
                            break;

                        case TipoDatoXLSX.CELULAR_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuNoCelularActual;
                            break;

                        case TipoDatoXLSX.EXTENSION:
                            ws.Cell(fila, columna).Value = usuario.UsuNoExtensionAnterior ?? string.Empty;
                            break;
                        case TipoDatoXLSX.EXTENSION_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuNoExtensionActual;
                            break;

                        case TipoDatoXLSX.AREA:
                            ws.Cell(fila, columna).Value = usuario.UsuIdAreaDepto;
                            break;
                        case TipoDatoXLSX.ID_EXTERNO:
                            ws.Cell(fila, columna).Value = id_externo_usuario;
                            break;
                    }
                }

                ws.Row(fila).Height = 60;

                // ADJUNTOS
                documentos_adjuntar = tipo_solicitud.GetDocumentos();

                //links.Add(GenerarLink(solicitud, TipoDocumento.CURP, ruta_usuario));

                if (tipo_personal is TipoPersonal.ALUMNO)
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.COM_INSCRIPCION, ruta_usuario));
                }

                if (documentos_adjuntar.Contains(TipoDocumento.CAP_BLOQUEO))
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_BLOQUEO, ruta_repositorio));
                }

                if (documentos_adjuntar.Contains(TipoDocumento.CAP_ANTIVIRUS))
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_ANTIVIRUS, ruta_repositorio));
                }

                if (documentos_adjuntar.Contains(TipoDocumento.CAP_ERROR))
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_ERROR, ruta_repositorio));
                }

                //SIGUIENTE FILA
                fila++;
            }//FOREACH SOLICITUD

            ws.Columns(1, columna_final).AdjustToContents();

            IXLRange registros = ws.Range(ws.Cell(5, 1), ws.Cell(4 + lista.Count(), columna_final));

            registros.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
            registros.Style.Border.SetOutsideBorderColor(XLColor.Black);

            registros.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
            registros.Style.Border.SetInsideBorderColor(XLColor.Black);

            registros.Style.Font.FontName = "Arial";
            registros.Style.Font.FontSize = 14;

            registros.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            registros.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            wb.SaveAs($"{ServerFS.GetBaseDir(true)}/{archivo_exportacion}");

            links.Add(new WebUtils.Link(archivo_exportacion, nombre_archivo));

            return links;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> LlenarFormulario(
                TExport<int[]> data,
                bool return_zip
            )
        {
            Guid guid = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = [] } ;

            string ruta = $"repositorio/pendientes/%ruta%{id_fecha}_{guid}";
            string ruta_exportacion = ruta.Replace("%ruta%", "EXPORTACION_");
            
            List<MtTbSolicitudesTicket> pendientes = [];
            List<MtTbSolicitudesTicket> pendientes_celular = [];
            List<MtTbSolicitudesTicket> pendientes_correo_personal = [];
            List<MtTbSolicitudesTicket> pendientes_desbloqueo = [];

            StringBuilder mensajes = new();

            List<WebUtils.Link> archivos = [];
            List<WebUtils.Link> exportados = [];

            List<Notificacion> notificaciones = [];

            string? error = null;

            try
            {
                pendientes = await _db.MtTbSolicitudesTickets
                    .Where
                    (
                        st => 
                            data.Value.Contains(st.IdSolicitudTicket) &&
                            st.SolIdEstadoSolicitud == data.Status
                    )
                    .Include(st => st.SolIdUsuarioNavigation)
                    .Include(st => st.SolIdTipoSolicitudNavigation)
                    .ToListAsync();

                mensajes.AppendLine($"PENDIENTES EN TOTAL: {pendientes.Count}");

                pendientes_celular = pendientes.Where(p => p.SolIdTipoSolicitud == (int)TipoSolicitud.CAMBIO_CELULAR).ToList();
                pendientes = pendientes.Except(pendientes_celular).ToList();

                mensajes.AppendLine($"PENDIENTES DE CAMBIO DE CELULAR: {pendientes_celular.Count}");

                pendientes_correo_personal = pendientes.Where(p => p.SolIdTipoSolicitud == (int)TipoSolicitud.CAMBIO_CORREO_PERSONAL).ToList();
                pendientes = pendientes.Except(pendientes_correo_personal).ToList();

                mensajes.AppendLine($"PENDIENTES DE CAMBIO DE CORREO PERSONAL: {pendientes_correo_personal.Count}");

                pendientes_desbloqueo = pendientes.Where(p => p.SolIdTipoSolicitud == (int)TipoSolicitud.DESBLOQUEO_CUENTA).ToList();
                pendientes = pendientes.Except(pendientes_desbloqueo).ToList();

                mensajes.AppendLine($"PENDIENTES DE DESBLOQUEO: {pendientes_desbloqueo.Count}");

                if (pendientes.Count>0)
                {
                    exportados = GenerarXLSX(pendientes, ruta);
                    archivos.AddRange(exportados);
                    pendientes.ForEach(p => p.SolIdEstadoSolicitud = data.Status);
                }
                
                if(pendientes_celular.Count>0)
                {
                    exportados = GenerarXLSX(pendientes_celular, ruta, TipoSolicitud.CAMBIO_CELULAR);
                    archivos.AddRange(exportados);
                    pendientes_celular.ForEach(p => p.SolIdEstadoSolicitud = data.Status);
                }

                if(pendientes_correo_personal.Count>0)
                {
                    exportados = GenerarXLSX(pendientes_correo_personal, ruta, TipoSolicitud.CAMBIO_CORREO_PERSONAL);
                    archivos.AddRange(exportados);
                    pendientes_correo_personal.ForEach(p => p.SolIdEstadoSolicitud = data.Status);
                }

                if (pendientes_desbloqueo.Count > 0)
                {
                    exportados = GenerarXLSX(pendientes_desbloqueo, ruta, TipoSolicitud.DESBLOQUEO_CUENTA);
                    archivos.AddRange(exportados);
                    pendientes_desbloqueo.ForEach(p => p.SolIdEstadoSolicitud = data.Status);
                }

                error = ServerFS.WriteZip($"{ruta_exportacion}.zip", archivos);

                if(error is not null)
                {
                    mensajes.AppendLine(error);
                }
                
                if (return_zip)
                {
                    WebUtils.Link link = new($"{ruta_exportacion}.zip");
                    link.Name = Path.GetFileName(link.Url);

                    oResponse.Data = new List<WebUtils.Link>() { link };
                }
                else
                {
                    oResponse.Data = archivos;
                }

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                mensajes.AppendLine(ex.Message);
                mensajes.AppendLine(ex.StackTrace);

                // Response.Clear();
                Response.StatusCode = 500;
            }

            if(oResponse.Success == 1)
            {
                try
                {
                    error = "";

                    if(data.Update)
                    {
                        if((TipoEstadoSolicitud)data.Status == TipoEstadoSolicitud.PENDIENTE)
                        {
                            data.Status = (int)TipoEstadoSolicitud.EN_PROCESO;
                        }

                        error = await EstablecerEstado(pendientes, data.Status);
                        error += await EstablecerEstado(pendientes_celular, data.Status);
                        error += await EstablecerEstado(pendientes_correo_personal, data.Status);
                        error += await EstablecerEstado(pendientes_desbloqueo, data.Status);

                        await _db.SaveChangesAsync();
                    }

                    if (string.IsNullOrEmpty(error) && data.Notify)
                    {
                        //TODO: Verificar
                        await EnvioMasivo(pendientes, data.Status);
                        await EnvioMasivo(pendientes_celular, data.Status);
                        await EnvioMasivo(pendientes_correo_personal, data.Status);
                        await EnvioMasivo(pendientes_desbloqueo, data.Status);
                    }
                    else
                    {
                        mensajes.AppendLine(error);
                    }
                }
                catch (Exception ex)
                {
                    mensajes.AppendLine(ex.Message);
                    mensajes.AppendLine(ex.StackTrace);
                }
            }

            if (mensajes.Length>0)
            {
                System.IO.File.WriteAllText($"{ServerFS.GetBaseDir(true)}/{ruta_exportacion}.log", mensajes.ToString());
                oResponse.Message = mensajes.ToString();
            }

            return Ok(oResponse);
        }//LLENAR FORMULARIO

        [HttpGet("*/arreglar_rotos")]
        public async Task<IActionResult> ListarEnlacesRotos()
        {
            Response<List<string>> oResponse = new() { Data = [] };
            List<MtTbSolicitudesTicket> solicitudes;
            
            string? enlace;
            
            string ruta_repositorio;
            string ruta_usuario;

            try
            {
                solicitudes = await _db.MtTbSolicitudesTickets
                    .Include(st => st.SolIdUsuarioNavigation)
                    .ToListAsync();

                foreach (MtTbSolicitudesTicket solicitud in solicitudes)
                {
                    ruta_repositorio = $"repositorio/solicitudes-tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                    ruta_usuario = $"repositorio/usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

                    enlace = EnlaceRoto(solicitud.SolIdUsuarioNavigation?.UsuFileNameCurp, ruta_usuario);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolIdUsuarioNavigation?.UsuFileNameComprobanteEstudios, ruta_usuario);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaCuentaBloqueada, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaEscaneoAntivirus, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaError, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);
                }

                oResponse.Success = 1;
            }
            catch(Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }
    }
}
