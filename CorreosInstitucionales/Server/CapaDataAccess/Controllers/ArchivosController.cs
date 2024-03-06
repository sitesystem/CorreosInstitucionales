using ClosedXML.Excel;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.Utils;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using System.Text;

using CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using Microsoft.AspNetCore.Components.Web;

using CorreosInstitucionales.Server.Correos;
using DocumentFormat.OpenXml.Wordprocessing;

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

        private async Task EnvioMasivo(List<MtTbSolicitudesTicket> lista)
        {
            ILoggerFactory loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            HtmlRenderer htmlRenderer = new HtmlRenderer(_serviceProvider, loggerFactory);

            RequestDTO_SendEmail correo = new RequestDTO_SendEmail()
            {
                Subject = "Su solcicitud ha sido canalizada hacia la mesa de control",
                EmailTo = "agmartinezc@ipn.mx",
                Body = "NO DEFINIDO"
            };

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                correo.Body = await htmlRenderer.Dispatcher.InvokeAsync
                (
                    async () =>
                    {

                        var parameters = Microsoft.AspNetCore.Components.ParameterView.FromDictionary
                        (
                            new Dictionary<string, object?>
                            {
                                { "nombre", solicitud.SolIdUsuarioNavigation.UsuNombre},
                                { "solicitud", solicitud.IdSolicitudTicket}
                            }
                        );

                        var output = await htmlRenderer.RenderComponentAsync<EnProceso>(parameters);

                        return output.ToHtmlString();
                    }
                );

                await _servicioCorreo.SendEmail(correo);
            }
        }

        private WebUtils.Link GenerarLink(MtTbSolicitudesTicket solicitud, int tipo_documento, string directorio)
        {
            string archivo = string.Empty;
            string tipo = string.Empty;
            
            string curp = solicitud.SolIdUsuarioNavigation.UsuCurp;
            string id = string.Format("{0:00000}", solicitud.IdSolicitudTicket);

            switch (tipo_documento)
            {
                case TipoDocumento.CURP:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameCurp!;
                    tipo = "CURP";
                    break;
                case TipoDocumento.COM_INSCRIPCION:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameComprobanteInscripcion!;
                    tipo = "COMPROBANTE_INSCRIPCION";
                    break;
                case TipoDocumento.CAP_ANTIVIRUS:
                    archivo = solicitud.SolCapturaEscaneoAntivirus!;
                    tipo = "CAPTURA_ANTIVIRUS";
                    break;
                case TipoDocumento.CAP_BLOQUEO:
                    archivo = solicitud.SolCapturaCuentaBloqueada!;
                    tipo = "CAPTURA_BLOQUEO";
                    break;
                case TipoDocumento.CAP_ERROR:
                    archivo = solicitud.SolCapturaError!;
                    tipo = "CAPTURA_ERROR";
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

        public async Task<IActionResult> LlenarFormulario(int[] selected, bool return_zip)
        {
            Guid id = Guid.NewGuid();
            string id_fecha = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            Response<List<WebUtils.Link>> oResponse = new() { Data = new() } ;
            List<MtTbSolicitudesTicket> pendientes = new List<MtTbSolicitudesTicket>();

            string base_directory = $"../Client/wwwroot";
            string filename = $"repositorio/pendientes/{id_fecha}_solicitud_alta_desbloqueo_{id}";
            string template_fn = $"assets/sol_alta_desbloqueo.xlsx";
            
            XLWorkbook wb = new XLWorkbook($"{base_directory}/{template_fn}");
            IXLWorksheet ws = wb.Worksheet(1);

            StringBuilder errors = new StringBuilder();
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
                pendientes = await Task.Run(
                    ()=>_db.MtTbSolicitudesTickets.
                        Where(st => selected.Contains(st.IdSolicitudTicket)).
                        Include(st=>st.SolIdUsuarioNavigation).
                        Include(st=>st.SolIdTipoSolicitudNavigation).
                        ToList()
                );

                foreach(MtTbSolicitudesTicket solicitud in pendientes)
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
                System.IO.File.WriteAllText($"{base_directory}/{filename}.txt", errors.ToString());

                oResponse.Message = errors.ToString();
                oResponse.Data.Add(new WebUtils.Link($"{base_directory}/{filename}.txt"));
            }

            return Ok(oResponse);
        }
    }
}
