using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EscuelasController : Controller
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatEscuela>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceCatEscuela>();

                if (filterByStatus)
                    list = await db.MceCatEscuelas.Where(e => e.EscStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await db.MceCatEscuelas.ToListAsync();

                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MceCatEscuela> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceCatEscuelas.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(EscuelaViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatEscuela oEscuela = new()
                {
                    IdEscuela = model.IdEscuela,
                    EscNoEscuela = model.EscNoEscuela,
                    EscNombreLargo = model.EscNombreLargo,
                    EscNombreCorto = model.EscNombreCorto,
                    EscLogo = model.EscLogo,
                    EscStatus = true
                };

                await db.MceCatEscuelas.AddAsync(oEscuela);
                await db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(EscuelaViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatEscuela? oEscuela = await db.MceCatEscuelas.FindAsync(model.IdEscuela);

                if (oEscuela != null)
                {
                    oEscuela.EscNoEscuela = model.EscNoEscuela;
                    oEscuela.EscNombreLargo = model.EscNombreLargo;
                    oEscuela.EscNombreCorto = model.EscNombreCorto;
                    oEscuela.EscLogo = model.EscLogo;
                    oEscuela.EscStatus = model.EscStatus;

                    db.Entry(oEscuela).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatEscuela? oEscuela = await db.MceCatEscuelas.FindAsync(id);
                //db.Remove(oPersona);

                if (oEscuela != null)
                {
                    oEscuela.EscStatus = isActivate;
                    db.Entry(oEscuela).State = EntityState.Modified;
                    await db.SaveChangesAsync();
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
