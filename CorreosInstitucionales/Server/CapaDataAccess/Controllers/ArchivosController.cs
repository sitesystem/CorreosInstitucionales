using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ArchivosController(DbCorreosInstitucionalesUpiicsaContext db) : Controller
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        private string GenerarExcel()
        {
            string filename = $"../Client/wwwroot/Repositorio/prueba.xlsx";

            XLWorkbook wb = new XLWorkbook();
            IXLWorksheet ws = wb.Worksheets.Add("Pendientes");

            ws.Cell("A2").Value = "Prueba";

            wb.SaveAs(filename);

            return filename;
        }

        [HttpGet("xlsx/pendientes")]
        public async Task<IActionResult> GenerarXLSX_Pendientes()
        {
            Response<string> oResponse = new();
            List<MpTbUsuario> pendientes = new List<MpTbUsuario>();

            try
            {
                oResponse.Data = await Task.Run(GenerarExcel);
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
