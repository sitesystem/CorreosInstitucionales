using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : Controller
    {
        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MceCatRole> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatRoles.FindAsync(id);
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

        [HttpPost]
        public async Task<IActionResult> AddData(RolViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatRole oRol = new()
                    {
                        IdRol = model.IdRol,
                        RolNombre = model.RolNombre,
                        RolDescripcion = model.RolDescripcion,
                    };
                    await db.MceCatRoles.AddAsync(oRol);
                    await db.SaveChangesAsync();

                    oResponse.Success = 1;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public IActionResult EditData(RolViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatRole? oRol = db.MceCatRoles.Find(model.IdRol);
                if (oRol != null)
                {
                    oRol.RolNombre = model.RolNombre;
                    oRol.RolDescripcion = model.RolDescripcion;

                    db.Entry(oRol).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }
    }
}
