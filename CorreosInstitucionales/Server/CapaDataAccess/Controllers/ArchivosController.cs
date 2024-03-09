using ClosedXML.Excel;
using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Server.Correos;
using CorreosInstitucionales.Shared.CapaEntities.Common;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers

{
    [Route("api/[controller]")]
    [ApiController]

    public class ArchivosController (
            DbCorreosInstitucionalesUpiicsaContext db, 
            ISendEmailService servicioEmail,
            IServiceProvider serviceProvider
        ) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;
        private readonly ISendEmailService _servicioCorreo = servicioEmail;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected string? EnlaceRoto(string? archivo, string ruta)
        {
            string root = Path.GetFullPath("../client/wwwroot/");
            string origen = $"{root}/assets/cheems.pdf";
            string destino = root + ruta + archivo;
            string dir = Path.GetFullPath(destino);

            bool enlace_roto =
            (
                archivo is not null &&
                archivo != "-" &&
                !System.IO.File.Exists(destino)
            );
            
            if(enlace_roto)
            {
                Directory.CreateDirectory(dir);
                System.IO.File.Copy(origen, destino, false );
            }

            return enlace_roto? ruta+archivo : null;
        }

        protected async Task<string?> EstablecerEstado(int[] lista_solicitudes, int estado)
        {
            string? response = null;

            try
            {
                List<MtTbSolicitudesTicket> solicitudes = await _db.MtTbSolicitudesTickets
                    .Where(ts => lista_solicitudes.Contains(ts.IdSolicitudTicket))
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

        private async Task EnvioMasivoRespuesta(List<MtTbSolicitudesTicket> lista)
        {
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

                switch (solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
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

                await _servicioCorreo.SendEmail(correo);
            }//FOREACH solicitud
        }

        private async Task EnvioMasivo(List<MtTbSolicitudesTicket> lista)
        {
            ComponentRenderer renderer = new ComponentRenderer(_serviceProvider);

            RequestDTO_SendEmail correo = new()
            {
                Subject = "Su solcicitud ha sido canalizada hacia la mesa de control",
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

                correo.Body = await renderer.GetHTML<EnProceso>(variables_correo);

                await _servicioCorreo.SendEmail(correo);
            }//FOREACH solicitud
        }// ENVIO  MASIVO (EN PROCESO)

        private WebUtils.Link GenerarLink(MtTbSolicitudesTicket solicitud, int tipo_documento, string directorio)
        {
            string archivo = string.Empty;
            string tipo = TipoDocumento.Nombre[tipo_documento] ?? "ARCHIVO";

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
        public async Task<IActionResult> GenerarPendientes_XLSX(int[] selected)
        {
            return await LlenarFormulario(selected, false);
        }

        [HttpPost("zip/pendientes")]
        public async Task<IActionResult> GenerarPendientes_ZIP(int[] selected)
        {
            return await LlenarFormulario(selected, true);
        }

        [HttpPost("xlsx/procesados")]
        public async Task<IActionResult> ImportarProcesados_XLSX(IFormFile file)
        {
            Response<string> oResponse = new();

            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            string filename = $"../Client/wwwroot/repositorio/procesados/{id_fecha}_solicitud_alta_desbloqueo_{id}"; // Development Root
            //string filename = $"wwwroot/repositorio/procesados/{id_fecha}_solicitud_alta_desbloqueo_{id}"; // Deployment Root

            StringBuilder sb = new StringBuilder();
            bool guardar_registro = false;

            //15,5
            Dictionary<string,RegistroImportacion> registros = new ();
            RegistroImportacion registro_actual;

            List<string> lista_curps = new();

            string CURP = string.Empty;
            
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

                            sb.AppendLine(registro_actual.ToString());
                        }
                    }//LEER XLSX
                    else
                    {
                        sb.AppendLine("EL ARCHIVO NO CUENTA CON REGISTROS.");
                        guardar_registro = true;
                    }

                    List<MtTbSolicitudesTicket> solicitudes = await _db.MtTbSolicitudesTickets
                        .Where
                        (
                            st =>
                                st.SolIdEstadoSolicitud == TipoEstadoSolicitud.EN_PROCESO &&
                                lista_curps.Contains(st.SolIdUsuarioNavigation.UsuCurp)
                        )
                        .Include(st => st.SolIdUsuarioNavigation)
                        .ToListAsync();

                    if(registros.Count != solicitudes.Count)
                    {
                        sb.AppendLine($"EL NÚMERO DE REGISTROS DEL ARCHIVO XLSX ({registros.Count}) NO COINCIDE CON EL NÚMERO DE SOLICITUDES ({solicitudes.Count}).");
                        guardar_registro = true;
                    }

                    //TODO: Cambio de asignaciones deacuerdo al tipo de solicitud
                    foreach(MtTbSolicitudesTicket solicitud in solicitudes)
                    {
                        registro_actual = registros[solicitud.SolIdUsuarioNavigation.UsuCurp];

                        solicitud.SolIdEstadoSolicitud = TipoEstadoSolicitud.ATENDIDA;

                        switch(solicitud.SolIdTipoSolicitud)
                        {
                            case TipoSolicitud.CAMBIO_CORREO_PERSONAL:
                                solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaAnterior = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                                solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva = registro_actual.CorreoPersonal;
                                break;

                            case TipoSolicitud.CAMBIO_CELULAR:
                                solicitud.SolIdUsuarioNavigation.UsuNoCelularAnterior = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo;
                                solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo = registro_actual.Celular;
                                break;

                            case TipoSolicitud.CORREO_EGRESADO://CAMBIO DE DOMINIO alumno.ipn.mx a egresado.ipn.mx
                            case TipoSolicitud.CREACION_ACTIVACION_CORREO_INST:
                                solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta = registro_actual.CorreoInstitucional;
                                solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContraseña = registro_actual.Clave;
                                break;

                            case TipoSolicitud.RECUPERACION_CONTRA:
                                solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContraseña = registro_actual.Clave;
                                break;

                            case TipoSolicitud.OTRO:
                                solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta = registro_actual.CorreoInstitucional;
                                solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalContraseña = registro_actual.Clave;

                                solicitud.SolIdUsuarioNavigation.UsuNoExtensionAnterior = solicitud.SolIdUsuarioNavigation.UsuNoExtension;
                                solicitud.SolIdUsuarioNavigation.UsuNoExtension = registro_actual.NoExtension;
                                break;
                        }
                        
                        solicitud.SolRespuestaDcyC = registro_actual.Accion;
                    }

                    await _db.SaveChangesAsync();

                    await EnvioMasivoRespuesta(solicitudes);
                }

                oResponse.Data = "OK";
                oResponse.Success = 1;
            }
            catch(Exception ex)
            {
                oResponse.Data = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            if (guardar_registro)
            {
                await System.IO.File.WriteAllTextAsync($"{filename}.log", sb.ToString());
                oResponse.Message += Environment.NewLine + sb.ToString();
            }

            return Ok(oResponse);
        }

        public async Task<IActionResult> LlenarFormulario(int[] selected, bool return_zip)
        {
            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = new() } ;
            List<MtTbSolicitudesTicket> pendientes = new List<MtTbSolicitudesTicket>();

            string base_directory = $"../Client/wwwroot"; // Development Root
            //string base_directory = $"wwwroot"; // Deployment Root
            string filename = $"repositorio/pendientes/{id_fecha}_solicitud_alta_desbloqueo_{id}";
            string template_fn = $"assets/sol_alta_desbloqueo.xlsx";
            
            XLWorkbook wb = new XLWorkbook($"{base_directory}/{template_fn}");
            IXLWorksheet ws = wb.Worksheet(1);

            StringBuilder errors = new();
            bool save_log = false;

            int i = 5;
            string rol = string.Empty;
            string ruta_repositorio = string.Empty;
            string ruta_usuario = string.Empty;

            List<WebUtils.Link> files = new();

            //Documentos adjuntos
            bool adjuntar_com_inscripcion = false;
            bool adjuntar_cap_antivirus = false;
            bool adjuntar_cap_bloqueo = false;
            bool adjuntar_cap_error = false;

            //FECHA
            ws.Cell("H2").Value = DateTime.Now.ToString("dd/MM/yyyy");

            string? id_externo_usuario = null;
            string? extension = null;
            string? error = null;

            try
            {
                pendientes = await _db.MtTbSolicitudesTickets
                        .Where(st => selected.Contains(st.IdSolicitudTicket))
                        .Include(st => st.SolIdUsuarioNavigation)
                        .Include(st => st.SolIdTipoSolicitudNavigation)
                        .ToListAsync();

                foreach (MtTbSolicitudesTicket solicitud in pendientes)
                {
                    ruta_repositorio = $"Repositorio/Solicitudes-Tickets/{solicitud.IdSolicitudTicket}/{solicitud.IdSolicitudTicket}_";
                    ruta_usuario = $"Repositorio/Usuarios/{solicitud.SolIdUsuario}/{solicitud.SolIdUsuario}_";

                    switch (solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
                    {
                        case TipoPersonal.ALUMNO: 
                        case TipoPersonal.EGRESADO: 
                            id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaAlumno;
                            adjuntar_com_inscripcion = true;
                            break;
                        case TipoPersonal.MAESTRIA: 
                            id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaMaestria;
                            adjuntar_com_inscripcion = true;
                            break;
                        default: 
                            id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuNumeroEmpleado;
                            extension = solicitud.SolIdUsuarioNavigation.UsuNoExtension;
                            break;
                    }

                    switch(solicitud.SolIdTipoSolicitud)
                    {
                        case TipoSolicitud.DESBLOQUEO_CUENTA:
                            adjuntar_cap_bloqueo = true;
                            adjuntar_cap_antivirus = true;
                            break;
                        case TipoSolicitud.OTRO:
                            adjuntar_cap_error = true;
                            break;
                    }

                    ws.Cell(i, 1).Value = solicitud.SolIdUsuarioNavigation.UsuNombre;
                    ws.Cell(i, 2).Value = solicitud.SolIdUsuarioNavigation.UsuPrimerApellido;
                    ws.Cell(i, 3).Value = solicitud.SolIdUsuarioNavigation.UsuSegundoApellido;
                    ws.Cell(i, 5).Value = solicitud.SolIdUsuarioNavigation.UsuCurp;
                    ws.Cell(i, 8).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                    ws.Cell(i, 9).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta;

                    ws.Cell(i, 4).Value = TipoPersonal.Nombre[solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal];
                    ws.Cell(i, 6).Value = id_externo_usuario;

                    ws.Cell(i, 7).Value = extension;

                    IXLRange row = ws.Range(ws.Cell(i, 1), ws.Cell(i, 9));

                    row.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    row.Style.Border.SetOutsideBorderColor(XLColor.Black);

                    row.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin);
                    row.Style.Border.SetInsideBorderColor(XLColor.Black);

                    files.Add(GenerarLink(solicitud, TipoDocumento.CURP, ruta_usuario));

                    if (adjuntar_com_inscripcion)
                    {
                        files.Add(GenerarLink(solicitud, TipoDocumento.COM_INSCRIPCION, ruta_usuario));
                    }

                    if(adjuntar_cap_bloqueo)
                    {
                        files.Add(GenerarLink(solicitud, TipoDocumento.CAP_BLOQUEO, ruta_repositorio));
                    }

                    if(adjuntar_cap_antivirus)
                    {
                        files.Add(GenerarLink(solicitud, TipoDocumento.CAP_ANTIVIRUS, ruta_repositorio));
                    }

                    if(adjuntar_cap_error)
                    {
                        files.Add(GenerarLink(solicitud, TipoDocumento.CAP_ERROR, ruta_repositorio));
                    }

                    i++;
                }

                ws.Columns(1, 10).AdjustToContents();

                wb.SaveAs($"{base_directory}/{filename}.xlsx");

                files.Add(new WebUtils.Link(filename + ".xlsx"));

                error = ServerFileSystem.WriteZip($"{base_directory}/{filename}.zip", files, base_directory + "/" );

                if(error is not null)
                {
                    errors.AppendLine(error);
                    save_log = true;
                }
                
                if (return_zip)
                {
                    oResponse.Data = new List<WebUtils.Link>() {  new WebUtils.Link(filename + ".zip") };
                }
                else
                {
                    oResponse.Data = files;
                }
                
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                errors.AppendLine(ex.Message);
                save_log = true;

                //Response.Clear();
                Response.StatusCode = 500;
            }

            if(oResponse.Success == 1)
            {
                try
                {
                    error = await EstablecerEstado(selected, TipoEstadoSolicitud.EN_PROCESO);
                    
                    if (error is null)
                    {
                        await EnvioMasivo(pendientes);
                    }
                    else
                    {
                        errors.AppendLine(error);
                        save_log = true;
                    }
                }
                catch (Exception ex)
                {
                    errors.AppendLine(ex.Message);
                    save_log = true;
                }
            }

            if (save_log)
            {
                System.IO.File.WriteAllText($"{base_directory}/{filename}.log", errors.ToString());

                oResponse.Message = errors.ToString();
                oResponse.Data.Add(new WebUtils.Link($"{base_directory}/{filename}.log"));
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
