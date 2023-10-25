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
    public class TiposPersonalController : Controller
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatTipoPersonal>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceCatTipoPersonal>();

                    if (filterByStatus)
                        list = await db.MceCatTipoPersonals.Where(e => e.TipoperStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceCatTipoPersonals.ToListAsync();

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

        [HttpGet("filterById/{id}")]
        public async Task<IActionResult> GetDataById(int id)
        {
            Response<MceCatTipoPersonal> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatTipoPersonals.FindAsync(id);
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
        public async Task<IActionResult> AddData(TipoPersonalViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatTipoPersonal oTipoPersonal = new()
                    {
                        IdTipoPersonal = model.IdTipoPersonal,
                        TipoperNombre = model.TipoperNombre,
                        TipoperDescripcion = model.TipoperDescripcion,
                        TipoperStatus = model.TipoperStatus
                    };
                    await db.MceCatTipoPersonals.AddAsync(oTipoPersonal);
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
        public async Task<IActionResult> EditData(TipoPersonalViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatTipoPersonal? oTipoPersonal = db.MceCatTipoPersonals.Find(model.IdTipoPersonal);
                if (oTipoPersonal != null)
                {
                    oTipoPersonal.TipoperNombre = model.TipoperNombre;
                    oTipoPersonal.TipoperDescripcion = model.TipoperDescripcion;
                    oTipoPersonal.TipoperStatus = model.TipoperStatus;

                    db.Entry(oTipoPersonal).State = EntityState.Modified;
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
                MceCatTipoPersonal? oTipoPersonal = db.MceCatTipoPersonals.Find(id);
                //db.Remove(oPersona);
                if (oTipoPersonal != null)
                {
                    oTipoPersonal.TipoperStatus = isActivate;
                    db.Entry(oTipoPersonal).State = EntityState.Modified;
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
