using ClosedXML.Excel;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ArchivosController(DbCorreosInstitucionalesUpiicsaContext db) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("xlsx/pendientes")]
        public async Task<IActionResult> GenerarXLSX_Pendientes()
        {
            Guid id = Guid.NewGuid();
            Response<string> oResponse = new();
            List<MtTbSolicitudesTicket> pendientes = new List<MtTbSolicitudesTicket>();

            string base_directory = $"../Client/wwwroot";
            string filename = $"repositorio/pendientes/pendiente_{id}.xlsx";

            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Pendientes");

            ws.Cell(1,1).Value = "ID SOL";
            ws.Cell(1,2).Value = "NOMBRE";
            ws.Cell(1,3).Value = "1ER APELLIDO";
            ws.Cell(1,4).Value = "2DO APELLIDO";
            ws.Cell(1,5).Value = "BOLETA";
            ws.Cell(1,6).Value = "TIPO SOLICITUD";
            ws.Cell(1,7).Value = "CTA NUEVA";
            ws.Cell(1,8).Value = "CTA ANTERIOR";
            ws.Cell(1,9).Value = "NO. CEL NUEVO";
            ws.Cell(1,10).Value = "NO. CEL ANTERIOR";

            int i = 2;
            string addr = string.Empty;

            try
            {
                pendientes = await Task.Run(
                    ()=>_db.MtTbSolicitudesTickets.
                        Where(st => st.SolIdEstadoSolicitud == 2).
                        Include(st=>st.SolIdUsuarioNavigation).
                        Include(st=>st.SolIdTipoSolicitudNavigation).
                        ToList()
                );

                foreach(MtTbSolicitudesTicket solicitud in pendientes)
                {
                    ws.Cell(i,1).Value = solicitud.IdSolicitudTicket;
                    ws.Cell(i,2).Value = solicitud.SolIdUsuarioNavigation.UsuNombre;
                    ws.Cell(i,3).Value = solicitud.SolIdUsuarioNavigation.UsuPrimerApellido;
                    ws.Cell(i,4).Value = solicitud.SolIdUsuarioNavigation.UsuSegundoApellido;
                    ws.Cell(i,5).Value = solicitud.SolIdUsuarioNavigation.UsuBoletaAlumno;
                    ws.Cell(i,6).Value = solicitud.SolIdTipoSolicitudNavigation.TiposolDescripcion;
                    ws.Cell(i,7).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                    ws.Cell(i,8).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaAnterior;
                    ws.Cell(i,9).Value = solicitud.SolIdUsuarioNavigation.UsuNoCelularNuevo;
                    ws.Cell(i,10).Value = solicitud.SolIdUsuarioNavigation.UsuNoCelularAnterior;
                    
                    i++;
                }

                ws.Columns(1, 10).AdjustToContents();

                wb.SaveAs($"{base_directory}/{filename}");

                oResponse.Data = filename;
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;

                Response.Clear();
                Response.StatusCode = 500;
            }

            return Ok(oResponse);
        }
    }
}
