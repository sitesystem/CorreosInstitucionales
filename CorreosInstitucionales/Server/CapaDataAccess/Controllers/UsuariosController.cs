using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public IActionResult GetAllData(bool filterByStatus)
        {
            Response<List<MlTbUsuariosSolicitante>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MlTbUsuariosSolicitante>();

                    if (filterByStatus)
                        //list = db.MlTbUsuariosSolicitante.Where(e => e.EdiStatus.Equals(filterByStatus)).ToList();
                        list = db.MlTbUsuariosSolicitantes.ToList();
                    else
                        list = db.MlTbUsuariosSolicitantes.ToList();

                    oResponse.Success = 1;
                    oResponse.Data = list;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }
    }
}
