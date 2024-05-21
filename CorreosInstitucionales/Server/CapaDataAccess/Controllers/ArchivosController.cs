using ClosedXML.Excel;
using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Server.Correos;
using CorreosInstitucionales.Server.MensajesWA;
using CorreosInstitucionales.Shared.CapaEntities.Common;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.Utils;
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

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers

{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]

    public class ArchivosController (
            DbCorreosInstitucionalesUpiicsaContext db, 
            ISendEmailService servicioEmail,
            IServiceProvider serviceProvider,
            HttpClient client
        ) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly ComponentRenderer renderer = new ComponentRenderer(serviceProvider);

        private readonly ISendEmailService _servicioCorreo = servicioEmail;
        private readonly WhatsApp WA = new WhatsApp(client);

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
        private async Task<List<string>> EnvioMasivoAtendidos(List<MtTbSolicitudesTicket> lista)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/procesados";
            List<string> errors = new();

            WhatsApp WA = new WhatsApp(client);

            //WA.SendMessage()
            
            RequestDTO_SendEmail correo = new()
            {
                Subject = "Su solcicitud ha sido atendida por la mesa de control",
                EmailTo = "agmartinezc@ipn.mx",
                Body = "NO DEFINIDO"
            };

            RequestDTO_SendWhatsApp mensaje = new()
            {
                Message = "PRUEBA",
                Number = "5500000000"
            };

            Dictionary<string, object?> variables_correo = new Dictionary<string, object?>
            {
                { "solicitud", null}
            };

            MtTbSolicitudesTicket[] solicitudes = lista.Where(s => s.SolValidacionDatos).ToArray();
            string dominio = string.Empty;

            foreach (MtTbSolicitudesTicket solicitud in solicitudes)
            {
                variables_correo["solicitud"] = solicitud;

                correo.EmailTo = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                mensaje.Number = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo.Replace(" ", string.Empty);

                // HACER ENVÍOS SIN ESPERARSE A SU RESULTADO
                if (!Dominios.EsCorreoDePrueba(correo.EmailTo))
                {
                    switch ((TipoPersonal)solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
                    {
                        case TipoPersonal.ALUMNO:
                        case TipoPersonal.EGRESADO:
                        case TipoPersonal.MAESTRIA:
                            correo.Body = await renderer.GetHTML<AtendidoAlumnoYEgresado>(variables_correo);
                            break;
                        default:
                            correo.Body = await renderer.GetHTML<AtendidoPersonal>(variables_correo);
                            break;
                    }

                    _ = Task.Run(() => _servicioCorreo.SendEmail(correo));
                }


                if(mensaje.Number != "5500000000")
                {
                    mensaje.Message = await renderer.GetHTML<AtendidoWA>(variables_correo);

                    Response<string> response = await WA.SendMessage(mensaje);
                    if (response.Success != 1)
                    {
                        errors.Add($"{mensaje.Number} :  {response.Message}");
                    }
                }
                
            }//FOREACH solicitud

            return errors;
        }        

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<List<string>> EnvioMasivoPendientes(List<MtTbSolicitudesTicket> lista, bool debug = false)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/pendientes";
            List<string> errors = new();

            
            WhatsApp WA = new WhatsApp(client);

            RequestDTO_SendEmail correo = new()
            {
                Subject = "Su solcicitud ha sido canalizada hacia la mesa de control",
                EmailTo = "postmaster@localhost",
                Body = "NO DEFINIDO"
            };

            RequestDTO_SendWhatsApp mensaje = new()
            {
                Message = "PRUEBA",
                Number = "5500000000"
            };

            Dictionary<string, object?> variables_correo = new Dictionary<string, object?>
            {
                { "solicitud", null}
            };

            int id_cambio_celular = (int)TipoSolicitud.CAMBIO_CELULAR;
            int id_cambio_correo = (int)TipoSolicitud.CAMBIO_CORREO_PERSONAL;

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                variables_correo["solicitud"] = solicitud;

                correo.EmailTo = solicitud.SolIdTipoSolicitud == id_cambio_correo ? solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaAnterior! : solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                mensaje.Number = solicitud.SolIdTipoSolicitud == id_cambio_celular ? solicitud.SolIdUsuarioNavigation.UsuNoCelularAnterior! : solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo;
                mensaje.Number.Replace(" ", string.Empty);

                // HACER ENVÍOS SIN ESPERARSE A SU RESULTADO
                if (!Dominios.EsCorreoDePrueba(correo.EmailTo))
                {
                    correo.Body = await renderer.GetHTML<EnProceso>(variables_correo);
                    _ = Task.Run(() => _servicioCorreo.SendEmail(correo));
                }

                if(mensaje.Number != "5500000000")
                {
                    mensaje.Message = await renderer.GetHTML<EnProcesoWA>(variables_correo);
                    Response<string> response = await WA.SendMessage(mensaje);
                    if (response.Success != 1)
                    {
                        errors.Add($"{mensaje.Number} :  {response.Message}");
                    }
                }

            }//FOREACH solicitud

            return errors;
        }// ENVIO  MASIVO (EN PROCESO)

        [ApiExplorerSettings(IgnoreApi = true)]
        private WebUtils.Link GenerarLink(MtTbSolicitudesTicket solicitud, TipoDocumento tipo_documento, string directorio)
        {
            string archivo = string.Empty;
            string tipo = tipo_documento.GetNombre();

            string curp = solicitud.SolIdUsuarioNavigation.UsuCurp;
            string id = string.Format("{0:00000}", solicitud.IdSolicitudTicket);

            switch (tipo_documento)
            {
                case TipoDocumento.CURP:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameCurp!;
                    break;
                case TipoDocumento.COM_INSCRIPCION:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameComprobanteInscripcion!;
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

            return new WebUtils.Link($"{directorio}{archivo}",$"SOL{id}_{curp}_{tipo}{ext}");
        }

        [HttpPost("xlsx/pendientes")]
        public async Task<IActionResult> ExportarPendientes_XLSX(int[] selected)
        {
            return await LlenarFormulario(selected, false);
        }

        [HttpPost("zip/pendientes")]
        public async Task<IActionResult> ExportarPendientes_ZIP(int[] selected)
        {
            return await LlenarFormulario(selected, true);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public TipoDatoXLSX GetTipoDato(string columna)
        {
            string colname = columna.Replace("Á", "A");
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

        [HttpPost("xlsx/procesados")]
        public async Task<IActionResult> ImportarProcesados_XLSX(IFormFile file)
        {
            Response<string> oResponse = new();

            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string basedir = ServerFS.GetBaseDir(true);
            string filename = $"{basedir}/repositorio/procesados/{id_fecha}_solicitud_alta_desbloqueo_{id}";
            
            List<string> logs = new();
            
            Dictionary<string,RegistroImportacion> registros = new ();
            RegistroImportacion registro_actual;

            List<string> lista_curps = new();

            string CURP = string.Empty;

            TipoDatoXLSX[] datos_a_actualizar = [];
            bool actualizar_todo = false;

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

                            registros.Add(CURP, registro_actual);
                        }

                        logs.Add("SE ENCONTRARON LAS SIGUIENTES COLUMNAS:");

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
                        logs.Add("EL ARCHIVO NO CUENTA CON REGISTROS.");
                    }
                    
                    List<MtTbSolicitudesTicket> solicitudes = await _db.MtTbSolicitudesTickets
                        .Where
                        (
                            st =>
                                st.SolIdEstadoSolicitud == ((int)TipoEstadoSolicitud.EN_PROCESO) &&
                                lista_curps.Contains(st.SolIdUsuarioNavigation.UsuCurp)
                        )
                        .Include(st => st.SolIdUsuarioNavigation)
                        .ToListAsync();

                    logs.Add($"{Environment.NewLine}SE ACTUALIZARÁN {solicitudes.Count} PENDIENTE{(solicitudes.Count == 1 ? string.Empty : "S")} DE {registros.Count} REGISTRO{(registros.Count == 1 ? string.Empty : "S")} DEL ARCHIVO...");
                    
                    //TODO: Cambio de asignaciones deacuerdo al tipo de solicitud
                    solicitudes.ForEach(solicitud =>
                    {
                        registro_actual = registros[solicitud.SolIdUsuarioNavigation.UsuCurp];
                        logs.Add(registro_actual.ToString());

                        datos_a_actualizar = ((TipoSolicitud)solicitud.SolIdTipoSolicitud).GetDatosActualizar();
                        actualizar_todo = datos_a_actualizar.Contains(TipoDatoXLSX.TODO);

                        if (datos_a_actualizar.Contains(TipoDatoXLSX.NINGUNO))
                        {
                            logs.Add($"\t - EL TIPO DE SOLICITUD NO REQUIRE ACTUALIZAR DATOS(?)");
                            return;
                        }
                        
                        if (string.IsNullOrEmpty(registro_actual.Accion))
                        {
                            logs.Add($"\t - ERROR: SE ESPERABA QUE LA COLUMNA DE ACCIÓN NO ESTÉ VACÍA.");
                            return;
                        }                        

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoXLSX.CORREO_PERSONAL))
                        {
                            if(string.IsNullOrEmpty(registro_actual.CorreoPersonal))
                            {
                                logs.Add($"\t - ERROR: {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE CORREO PERSONAL NO ESTÉ VACÍA.");
                                return;
                            }
                            solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaAnterior = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                            solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva = registro_actual.CorreoPersonal;

                            logs.Add($"\t - SE ACTUALIZÓ EL CORREO PERSONAL");
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoXLSX.CORREO_INSTITUCIONAL))
                        {
                            if (string.IsNullOrEmpty(registro_actual.CorreoInstitucional))
                            {
                                logs.Add($"\t - ERROR: {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE CORREO INSTITUCIONAL NO ESTÉ VACÍA.");
                                return;
                            }
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta = registro_actual.CorreoInstitucional;
                            logs.Add($"\t - SE ACTUALIZÓ EL CORREO INSTITUCIONAL");
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoXLSX.CONTRA))
                        {
                            if (string.IsNullOrEmpty(registro_actual.Clave))
                            {
                                logs.Add($"\t - ERROR: {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE CONTRASEÑA NO ESTÉ VACÍA.");
                                return;
                            }
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContraseña = registro_actual.Clave;
                            logs.Add($"\t - SE ACTUALIZÓ LA CONTRASEÑA");
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoXLSX.CELULAR))
                        {
                            if (string.IsNullOrEmpty(registro_actual.Celular))
                            {
                                logs.Add($"\t - ERROR: {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE NÚMERO DE CELULAR NO ESTÉ VACÍA.");
                                return;
                            }

                            solicitud.SolIdUsuarioNavigation.UsuNoCelularAnterior = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo;
                            solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo = registro_actual.Celular;

                            logs.Add($"\t - SE ACTUALIZÓ EL NÚMERO DE CELULAR");
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoXLSX.EXTENSION))
                        {
                            if (string.IsNullOrEmpty(registro_actual.NoExtension    ))
                            {
                                logs.Add($"\t - ERROR: {registro_actual.CURP} SE ESPERABA QUE LA COLUMNA DE NÚMERO DE EXTENSIÓN NO ESTÉ VACÍA.");
                                return;
                            }

                            solicitud.SolIdUsuarioNavigation.UsuNoExtensionAnterior = solicitud.SolIdUsuarioNavigation.UsuNoExtension;
                            solicitud.SolIdUsuarioNavigation.UsuNoExtension = registro_actual.NoExtension;

                            logs.Add($"\t - SE ACTUALIZÓ EL NÚMERO DE EXTENSIÓN");
                        }

                        solicitud.SolRespuestaDcyC = registro_actual.Accion;
                        solicitud.SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.ATENDIDA;
                        solicitud.SolValidacionDatos = true;
                    });

                    await _db.SaveChangesAsync();

                    await EnvioMasivoAtendidos(solicitudes);
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
        public List<WebUtils.Link> GenerarXLSX(IEnumerable<MtTbSolicitudesTicket> lista, string archivo, TipoSolicitud formato_solicitud = TipoSolicitud.NO_ESPECIFICADA)
        {
            List<WebUtils.Link> links = new();

            // ARCHIVOS
            string plantilla = $"{ServerFS.GetBaseDir(true)}/assets/{formato_solicitud.GetPlantilla()}";
            string archivo_exportacion = $"{archivo}_{formato_solicitud.GetNombreExportacion()}";
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
            
            TipoDatoXLSX[] datos_exportar = formato_solicitud.GetDatosExportar();
            TipoDocumento[] documentos_adjuntar;
            TipoSolicitud tipo_solicitud;

            Dictionary<TipoDatoXLSX, int> columnas = new Dictionary<TipoDatoXLSX, int>();
            int fila = 5; //PRIMER FILA A LEER
            int columna = 5;//PRIMER COLUMNA DISPONIBLE
            int columna_final = 1;

            foreach(TipoDatoXLSX dato in datos_exportar)
            {
                columnas.Add(dato, columna);
                columna++;
            }
                        
            columna_final = columna - 1;

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                ruta_repositorio = $"repositorio/solicitudes-tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                ruta_usuario = $"repositorio/usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

                usuario = solicitud.SolIdUsuarioNavigation!;

                id_externo_usuario = usuario.UsuNumeroEmpleado;
                
                tipo_personal = (TipoPersonal)usuario.UsuIdTipoPersonal;
                tipo_solicitud = (TipoSolicitud)solicitud.SolIdTipoSolicitud;

                switch (tipo_personal)
                {
                    case TipoPersonal.ALUMNO:
                    case TipoPersonal.EGRESADO:
                        id_externo_usuario = usuario.UsuBoletaAlumno;
                        break;
                    case TipoPersonal.MAESTRIA:
                        id_externo_usuario = usuario.UsuBoletaMaestria;
                        break;
                }

                ws.Cell(fila, 1).Value = FormatoTexto(usuario.UsuNombre);
                ws.Cell(fila, 2).Value = FormatoTexto(usuario.UsuPrimerApellido);
                ws.Cell(fila, 3).Value = FormatoTexto(usuario.UsuSegundoApellido);
                ws.Cell(fila, 4).Value = ((TipoPersonal)usuario.UsuIdTipoPersonal).GetNombre();

                foreach(TipoDatoXLSX dato in datos_exportar)
                {
                    columna = columnas[dato];
                    switch(dato)
                    {
                        case TipoDatoXLSX.CORREO_PERSONAL: 
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoPersonalCuentaAnterior; 
                            break;
                        case TipoDatoXLSX.CORREO_INSTITUCIONAL:
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoInstitucionalCuenta;
                            break;
                        case TipoDatoXLSX.CELULAR:
                            ws.Cell(fila, columna).Value = usuario.UsuNoCelularAnterior;
                            break;
                        case TipoDatoXLSX.EXTENSION:
                            ws.Cell(fila, columna).Value = usuario.UsuNoExtensionAnterior;
                            break;
                        case TipoDatoXLSX.AREA:
                            ws.Cell(fila, columna).Value = usuario.UsuIdAreaDepto;
                            break;
                        case TipoDatoXLSX.ID_EXTERNO:
                            ws.Cell(fila, columna).Value = id_externo_usuario;
                            break;
                        case TipoDatoXLSX.CURP:
                            ws.Cell(fila, columna).Value = usuario.UsuCurp;
                            break;

                        case TipoDatoXLSX.CORREO_PERSONAL_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuCorreoPersonalCuentaNueva;
                            break;
                        case TipoDatoXLSX.CELULAR_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuNoCelularNuevo;
                            break;
                        case TipoDatoXLSX.EXTENSION_NUEVO:
                            ws.Cell(fila, columna).Value = usuario.UsuNoExtension;
                            break;
                    }
                }

                /*IXLRange row = ws.Range(ws.Cell(i, 1), ws.Cell(i, cambio_celular ? 11 : 9));*/

                IXLRange registros = ws.Range(ws.Cell(fila, 1), ws.Cell(fila, columna_final));

                registros.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                registros.Style.Border.SetOutsideBorderColor(XLColor.Black);

                registros.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                registros.Style.Border.SetInsideBorderColor(XLColor.Black);

                registros.Style.Font.FontName = "Arial";
                registros.Style.Font.FontSize = 14;

                registros.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                registros.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);


                ws.Row(fila).Height = 60;

                // ADJUNTOS
                documentos_adjuntar = tipo_solicitud.GetDocumentos();

                links.Add(GenerarLink(solicitud, TipoDocumento.CURP, ruta_usuario));

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
            }//FOR

            ws.Columns(1, 11).AdjustToContents();

            wb.SaveAs($"{ServerFS.GetBaseDir(true)}/{archivo_exportacion}");

            links.Add(new WebUtils.Link(archivo_exportacion, nombre_archivo));

            return links;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> LlenarFormulario(int[] selected, bool return_zip)
        {
            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = new() } ;

            string archivo = $"repositorio/pendientes/{id_fecha}_{id}";

            List<MtTbSolicitudesTicket> pendientes = new();
            List<MtTbSolicitudesTicket> pendientes_celular = new();
            List<MtTbSolicitudesTicket> pendientes_correo_personal = new();

            StringBuilder mensajes = new();

            List<WebUtils.Link> archivos = new();
            List<WebUtils.Link> exportados = new();

            string? error = null;

            int id_estado_proceso = (int)TipoEstadoSolicitud.EN_PROCESO;

            try
            {
                pendientes = await _db.MtTbSolicitudesTickets
                    .Where
                    (
                        st => 
                            selected.Contains(st.IdSolicitudTicket) &&
                            st.SolIdEstadoSolicitud == (int)TipoEstadoSolicitud.PENDIENTE
                    )
                    .Include(st => st.SolIdUsuarioNavigation)
                    .Include(st => st.SolIdTipoSolicitudNavigation)
                    .ToListAsync();

                pendientes_celular = pendientes.Where(p => p.SolIdTipoSolicitud == (int)TipoSolicitud.CAMBIO_CELULAR).ToList();
                pendientes = pendientes.Except(pendientes_celular).ToList();

                pendientes_correo_personal = pendientes.Where(p => p.SolIdTipoSolicitud == (int)TipoSolicitud.CAMBIO_CORREO_PERSONAL).ToList();
                pendientes = pendientes.Except(pendientes_correo_personal).ToList();

                

                if(pendientes.Count>0)
                {
                    exportados = GenerarXLSX(pendientes, archivo);
                    archivos.AddRange(exportados);
                    pendientes.ForEach(p => p.SolIdEstadoSolicitud = id_estado_proceso);
                }
                
                if(pendientes_celular.Count>0)
                {
                    exportados = GenerarXLSX(pendientes_celular, archivo, TipoSolicitud.CAMBIO_CELULAR);
                    archivos.AddRange(exportados);
                    pendientes_celular.ForEach(p => p.SolIdEstadoSolicitud = id_estado_proceso);
                }

                if(pendientes_correo_personal.Count>0)
                {
                    exportados = GenerarXLSX(pendientes_correo_personal, archivo, TipoSolicitud.CAMBIO_CORREO_PERSONAL);
                    archivos.AddRange(exportados);
                    pendientes_correo_personal.ForEach(p => p.SolIdEstadoSolicitud = id_estado_proceso);
                }

                error = ServerFS.WriteZip($"{archivo}.zip", archivos);

                if(error is not null)
                {
                    mensajes.AppendLine(error);
                }
                
                if (return_zip)
                {
                    oResponse.Data = new List<WebUtils.Link>() {  new WebUtils.Link($"{archivo}.zip") };
                }
                else
                {
                    oResponse.Data = archivos;
                }

                await _db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                mensajes.AppendLine(ex.Message);
                mensajes.AppendLine(ex.StackTrace);

                //Response.Clear();
                Response.StatusCode = 500;
            }

            if(oResponse.Success == 1)
            {
                try
                {
                    error = await EstablecerEstado(pendientes, (int)TipoEstadoSolicitud.EN_PROCESO);
                    
                    if (error is null)
                    {
                        await EnvioMasivoPendientes(pendientes);
                        await EnvioMasivoPendientes(pendientes_celular);
                        await EnvioMasivoPendientes(pendientes_correo_personal);
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
                System.IO.File.WriteAllText($"{ServerFS.GetBaseDir(true)}/{archivo}.log", mensajes.ToString());
                oResponse.Message = mensajes.ToString();
            }

            return Ok(oResponse);
        }//LLENAR FORMULARIO

        [HttpGet("*/arreglar_rotos")]
        public async Task<IActionResult> ListarEnlacesRotos()
        {
            Response<List<string>> oResponse = new() { Data = new() };
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

                    enlace = EnlaceRoto(solicitud.SolIdUsuarioNavigation.UsuFileNameCurp, ruta_usuario);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolIdUsuarioNavigation.UsuFileNameComprobanteInscripcion, ruta_usuario);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaCuentaBloqueada, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaEscaneoAntivirus, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);

                    enlace = EnlaceRoto(solicitud.SolCapturaError, ruta_repositorio);
                    if (enlace is not null) oResponse.Data.Add(enlace);
                }

                oResponse.Success = 1;
            }catch(Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }
    }
}
