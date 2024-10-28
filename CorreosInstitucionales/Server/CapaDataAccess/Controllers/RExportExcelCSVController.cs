using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RExportExcelCSVController(DbCorreosInstitucionalesUpiicsaContext db) : ExportExcelCSVController
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("export/usuarios/csv")]
        public FileStreamResult ExportCategoriesToCSV()
        {
            return ToCSV(ApplyQuery(_db.MpTbUsuarios, Request.Query));
        }

        [HttpGet("export/usuarios/excel")]
        public FileStreamResult ExportCategoriesToExcel()
        {
            return ToExcel(ApplyQuery(_db.MpTbUsuarios, Request.Query));
        }
    }
}
