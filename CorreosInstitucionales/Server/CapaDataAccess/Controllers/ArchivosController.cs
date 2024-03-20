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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers

{
    [Route("api/[controller]")]
    [ApiController]

    public class ArchivosController (
            DbCorreosInstitucionalesUpiicsaContext db, 
            ISendEmailService servicioEmail,
            IServiceProvider serviceProvider,
            ISendWhatsAppService servicioWA
        ) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly ISendEmailService _servicioCorreo = servicioEmail;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ISendWhatsAppService _servicioWA = servicioWA;

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
        private async Task EnvioMasivoRespuesta(List<MtTbSolicitudesTicket> lista, bool debug = true)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/procesados";

            ComponentRenderer renderer = new ComponentRenderer(_serviceProvider);
            
            RequestDTO_SendEmail correo = new()
            {
                Subject = "Su solcicitud ha sido atendida por la mesa de control",
                EmailTo = "agmartinezc@ipn.mx",
                Body = "NO DEFINIDO"
            };

            Dictionary<string, object?> variables_correo = new Dictionary<string, object?>
            {
                { "solicitud", null}
            };

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                variables_correo["solicitud"] = solicitud;

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

                if (debug)
                {
                    System.IO.File.WriteAllText($"{archivo}/{id}_{solicitud.IdSolicitudTicket}.html", correo.Body);
                }

                await _servicioCorreo.SendEmail(correo);
                
            }//FOREACH solicitud
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task EnvioMasivoPendientes(List<MtTbSolicitudesTicket> lista, bool debug = true)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/pendientes";

            ComponentRenderer renderer = new ComponentRenderer(_serviceProvider);

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

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                variables_correo["solicitud"] = solicitud;

                correo.Body = await renderer.GetHTML<EnProceso>(variables_correo);
                mensaje.Message = await renderer.GetHTML<EnProcesoWA>(variables_correo);

                if(debug)
                {
                    System.IO.File.WriteAllText($"{archivo}/{id}_{solicitud.IdSolicitudTicket}.html", correo.Body);
                    System.IO.File.WriteAllText($"{archivo}/{id}_{solicitud.IdSolicitudTicket}.txt", mensaje.Message);
                }


                // HACER ENVÍOS SIN ESPERARSE A SU RESULTADO
                _ = Task.Run(() => _servicioCorreo.SendEmail(correo));
                _ = Task.Run(() => _servicioWA.SendWhatsAppAsync(mensaje));
            }//FOREACH solicitud
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

            TipoDatoActualizar[] datos_a_actualizar = [];
            bool actualizar_todo = false;

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    XLWorkbook wb = new XLWorkbook(stream);
                    IXLWorksheet ws = wb.Worksheet(1);

                    int n = ws.LastRowUsed().RowNumber();

                    if (n > 4)
                    {
                        for(int row = 5; row<=n; row++)
                        {
                            CURP = ws.Cell(row, 5).Value.ToString().Trim();
                            
                            lista_curps.Add(CURP);

                            registro_actual = new RegistroImportacion()
                            {
                                CURP = CURP,
                                ID = ws.Cell(row, 6).Value.ToString(),
                                NoExtension = ws.Cell(row, 7).Value.ToString(),
                                CorreoPersonal = ws.Cell(row, 8).Value.ToString(),
                                CorreoInstitucional = ws.Cell(row, 9).Value.ToString(),
                                Clave = ws.Cell(row, 10).Value.ToString(),
                                Accion = ws.Cell(row, 11).Value.ToString()
                            };

                            registros.Add(CURP, registro_actual);

                            logs.Add(registro_actual.ToString());
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
                                st.SolIdEstadoSolicitud == (int)TipoEstadoSolicitud.EN_PROCESO &&
                                lista_curps.Contains(st.SolIdUsuarioNavigation.UsuCurp)
                        )
                        .Include(st => st.SolIdUsuarioNavigation)
                        .ToListAsync();

                    logs.Add($"SE ACTUALIZ{(registros.Count > 1 ? "ARÁN" : "Á")} {registros.Count} SOLICITUD{(registros.Count>1?"ES":string.Empty)} DE {solicitudes.Count} PENDIENTE{(solicitudes.Count>1?"S":string.Empty)}...");                    

                    //TODO: Cambio de asignaciones deacuerdo al tipo de solicitud
                    foreach (MtTbSolicitudesTicket solicitud in solicitudes)
                    {
                        datos_a_actualizar = ((TipoSolicitud)solicitud.SolIdTipoSolicitud).GetDatosActualizar();

                        solicitud.SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.ATENDIDA;

                        if (datos_a_actualizar.Contains(TipoDatoActualizar.NINGUNO))
                        {
                            continue;
                        }

                        registro_actual = registros[solicitud.SolIdUsuarioNavigation.UsuCurp];

                        actualizar_todo = datos_a_actualizar.Contains(TipoDatoActualizar.TODO);

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoActualizar.CORREO_PERSONAL))
                        {
                            solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaAnterior = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                            solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva = registro_actual.CorreoPersonal;
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoActualizar.CORREO_INSTITUCIONAL))
                        {
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta = registro_actual.CorreoInstitucional;
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoActualizar.CONTRA))
                        {
                            solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContraseña = registro_actual.Clave;
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoActualizar.CELULAR))
                        {
                            solicitud.SolIdUsuarioNavigation.UsuNoCelularAnterior = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo;
                            solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo = registro_actual.Celular;
                        }

                        if (actualizar_todo || datos_a_actualizar.Contains(TipoDatoActualizar.EXTENSION))
                        {
                            solicitud.SolIdUsuarioNavigation.UsuNoExtensionAnterior = solicitud.SolIdUsuarioNavigation.UsuNoExtension;
                            solicitud.SolIdUsuarioNavigation.UsuNoExtension = registro_actual.NoExtension;
                        }
                                                
                        solicitud.SolRespuestaDcyC = registro_actual.Accion;
                    }

                    await _db.SaveChangesAsync();

                    await EnvioMasivoRespuesta(solicitudes);
                }

                oResponse.Success = 1;
            }
            catch(Exception ex)
            {
                oResponse.Data = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            string log_content = string.Join(Environment.NewLine, logs);

            await System.IO.File.WriteAllTextAsync($"{filename}.log", log_content);

            oResponse.Data = log_content;

            return Ok(oResponse);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<WebUtils.Link> GenerarXLSX(string plantilla, IEnumerable<MtTbSolicitudesTicket> lista, string archivo, bool cambio_celular = false)
        {
            List<WebUtils.Link> links = new();

            // EXCEL
            XLWorkbook wb = new XLWorkbook(plantilla);
            IXLWorksheet ws = wb.Worksheet(1);
            
            int i = 5; //PRIMER FILA A LEER
            
            ws.Cell("H2").Value = DateTime.Now.ToString("dd/MM/yyyy");//FECHA
            

            // ARCHIVOS
            string ruta_repositorio = string.Empty;
            string ruta_usuario = string.Empty;

            // ADJUNTOS
            bool adjuntar_com_inscripcion = false;
            bool adjuntar_cap_antivirus = false;
            bool adjuntar_cap_bloqueo = false;
            bool adjuntar_cap_error = false;

            // DATOS USUARIO
            MpTbUsuario usuario;

            // DATOS SOLICITUD
            string? id_externo_usuario = null;
            string? extension = null;
            
            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                ruta_repositorio = $"Repositorio/Solicitudes-Tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                ruta_usuario = $"Repositorio/Usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

                usuario = solicitud.SolIdUsuarioNavigation!;

                id_externo_usuario = usuario.UsuNumeroEmpleado;
                extension = usuario.UsuNoExtension;

                switch ((TipoPersonal)usuario.UsuIdTipoPersonal)
                {
                    case TipoPersonal.ALUMNO:
                    case TipoPersonal.EGRESADO:
                        id_externo_usuario = usuario.UsuBoletaAlumno;
                        adjuntar_com_inscripcion = true;
                        break;
                    case TipoPersonal.MAESTRIA:
                        id_externo_usuario = usuario.UsuBoletaMaestria;
                        adjuntar_com_inscripcion = true;
                        break;
                }

                switch ((TipoSolicitud)solicitud.SolIdTipoSolicitud)
                {
                    case TipoSolicitud.DESBLOQUEO_CUENTA:
                        adjuntar_cap_bloqueo = true;
                        adjuntar_cap_antivirus = true;
                        break;
                    case TipoSolicitud.OTRO:
                        adjuntar_cap_error = true;
                        break;
                }

                ws.Cell(i, 1).Value = usuario.UsuNombre;
                ws.Cell(i, 2).Value = usuario.UsuPrimerApellido;
                ws.Cell(i, 3).Value = usuario.UsuSegundoApellido;
                ws.Cell(i, 4).Value = ((TipoPersonal)usuario.UsuIdTipoPersonal).GetNombre();
                ws.Cell(i, 5).Value = usuario.UsuCurp;
                ws.Cell(i, 6).Value = id_externo_usuario;//BOLETA (DE MAESTÍRA) | NÚMERO DE CONTRATO
                ws.Cell(i, 7).Value = extension; // NUEVA?
                ws.Cell(i, 8).Value = usuario.UsuCorreoPersonalCuentaNueva;
                ws.Cell(i, 9).Value = usuario.UsuCorreoInstitucionalCuenta;

                if(cambio_celular)
                {
                    ws.Cell(i, 10).Value = usuario.UsuNoCelularNuevo;
                    ws.Cell(i, 11).Value = usuario.UsuNoCelularAnterior;
                }

                IXLRange row = ws.Range(ws.Cell(i, 1), ws.Cell(i, cambio_celular ? 11 : 9));

                row.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                row.Style.Border.SetOutsideBorderColor(XLColor.Black);

                row.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                row.Style.Border.SetInsideBorderColor(XLColor.Black);

                // ADJUNTOS
                links.Add(GenerarLink(solicitud, TipoDocumento.CURP, ruta_usuario));

                if (adjuntar_com_inscripcion)
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.COM_INSCRIPCION, ruta_usuario));
                }

                if (adjuntar_cap_bloqueo)
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_BLOQUEO, ruta_repositorio));
                }

                if (adjuntar_cap_antivirus)
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_ANTIVIRUS, ruta_repositorio));
                }

                if (adjuntar_cap_error)
                {
                    links.Add(GenerarLink(solicitud, TipoDocumento.CAP_ERROR, ruta_repositorio));
                }

                i++;
            }

            ws.Columns(1, 11).AdjustToContents();

            wb.SaveAs($"{ServerFS.GetBaseDir(true)}/{archivo}");

            links.Add(new WebUtils.Link(archivo));

            return links;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> LlenarFormulario(int[] selected, bool return_zip)
        {
            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = new() } ;

            string root = ServerFS.GetBaseDir(true);
            string archivo = $"repositorio/pendientes/{id_fecha}_{id}";

            string xlsx_normal = $"{root}/assets/sol_alta_desbloqueo.xlsx";
            string xlsx_celular = $"{root}/assets/sol_cambio_celular.xlsx";

            List<MtTbSolicitudesTicket> pendientes = new();
            List<MtTbSolicitudesTicket> pendientes_normal = new();
            List<MtTbSolicitudesTicket> pendientes_celular = new();
            
            StringBuilder mensajes = new();

            List<WebUtils.Link> archivos_normal = new();
            List<WebUtils.Link> archivos_celular = new();
            List<WebUtils.Link> archivos = new();

            string? error = null;

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
                pendientes_normal = pendientes.Except(pendientes_celular).ToList();

                if(pendientes_normal.Count>0)
                {
                    archivos_normal = GenerarXLSX(xlsx_normal, pendientes_normal, $"{archivo}_N.xlsx");
                }
                

                if(pendientes_celular.Count>0)
                {
                    archivos_celular = GenerarXLSX(xlsx_celular, pendientes_celular, $"{archivo}_C.xlsx", true);
                }                

                archivos = archivos_normal.Union(archivos_celular).ToList();
                
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
                System.IO.File.WriteAllText($"{root}/{archivo}.log", mensajes.ToString());
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
                    ruta_repositorio = $"Repositorio/Solicitudes-Tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                    ruta_usuario = $"Repositorio/Usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

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
