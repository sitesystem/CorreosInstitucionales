using ClosedXML.Excel;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;
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
            string template_fn = $"assets/sol_alta_desbloqueo.xlsx";

            XLWorkbook wb = new XLWorkbook($"{base_directory}/{template_fn}");
            IXLWorksheet ws = wb.Worksheet(1);

            /*
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
            */

            int i = 5;
            string rol = string.Empty;

            //FECHA
            ws.Cell("H2").Value = DateTime.Now.ToString("dd/MM/yyyy");

            /* ROLES - PSOIBLES VALORES:
             * ALUMNO
             * EGRESADO
             * DOCENTE
             * ADMINISTRATIVO
             * FUNCIONARIO
             * HONORARIOS
            */

            Dictionary<int, string> roles = new()
            {
                {1, "ALUMNO" },//ALUMNO
                {2, "EGRESADO" },//EGRESADO
                {3, "EGRESADO" },//ALUMNO MAESTRIA
                {4, "ADMINISTRATIVO" },//PAAE (ADMINISTRATIVO)
                {5, "DOCENTE" },//DOCENTE
                {6, "HONORARIOS" },//HONORARIOS
                {7, "FUNCIONARIO" },//PERSONAL UDI - <!> PREGUNTAR
            };

            string? id_externo_usuario = string.Empty;

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
                    switch (solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal)
                    {
                        case 1: case 2: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaAlumno; break;
                        case 3: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuBoletaMaestria; break;
                        default: id_externo_usuario = solicitud.SolIdUsuarioNavigation.UsuNumeroEmpleado; break;
                    }

                    ws.Cell(i, 1).Value = solicitud.SolIdUsuarioNavigation.UsuNombre;
                    ws.Cell(i, 2).Value = solicitud.SolIdUsuarioNavigation.UsuPrimerApellido;
                    ws.Cell(i, 3).Value = solicitud.SolIdUsuarioNavigation.UsuSegundoApellido;
                    ws.Cell(i, 5).Value = solicitud.SolIdUsuarioNavigation.UsuCurp;
                    ws.Cell(i, 8).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaNueva;
                    ws.Cell(i, 9).Value = solicitud.SolIdUsuarioNavigation.UsuCorreoInstitucionalCuenta;

                    ws.Cell(i, 4).Value = roles[solicitud.SolIdUsuarioNavigation.UsuIdTipoPersonal];
                    ws.Cell(i, 6).Value = id_externo_usuario;

                    ws.Cell(i, 7).Value = solicitud.SolIdUsuarioNavigation.UsuNoExtension;

                    IXLRange row = ws.Range(ws.Cell(i, 1), ws.Cell(i, 9));

                    row.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin);
                    row.Style.Border.SetOutsideBorderColor(XLColor.Red);
                    
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
