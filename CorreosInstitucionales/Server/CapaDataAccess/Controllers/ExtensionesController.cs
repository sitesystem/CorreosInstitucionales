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
    public class ExtensionesController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatExtension>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceCatExtension>();

                if (filterByStatus)
                    list = await db.MceCatExtensiones.Where(e => e.ExtStatus.Equals(filterByStatus))
                                                     .Include(e => e.ExtIdAreaDeptoNavigation)
                                                     .ThenInclude(a => a.AreIdEdificioNavigation)
                                                     .Include(e => e.ExtIdAreaDeptoNavigation)
                                                     .ThenInclude(a => a.AreIdPisoNavigation)
                                                     .ToListAsync();
                else
                    list = await db.MceCatExtensiones
                                   .Include(e => e.ExtIdAreaDeptoNavigation)
                                   .ThenInclude(a => a.AreIdEdificioNavigation)
                                   .Include(e => e.ExtIdAreaDeptoNavigation)
                                   .ThenInclude(a => a.AreIdPisoNavigation)
                                   .ToListAsync();

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
            Response<MceCatExtension> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceCatExtensiones
                                   .Include(e => e.ExtIdAreaDeptoNavigation)
                                   .ThenInclude(a => a.AreIdEdificioNavigation)
                                   .Include(e => e.ExtIdAreaDeptoNavigation)
                                   .ThenInclude(a => a.AreIdPisoNavigation)
                                   .FirstOrDefaultAsync(e => e.IdExtension == id);

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
        public async Task<IActionResult> AddData(ExtensionViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatExtension oExtension = new()
                {
                    IdExtension = model.IdExtension,
                    ExtNoExtension = model.ExtNoExtension,
                    ExtIdAreaDepto = model.ExtIdAreaDepto,
                    ExtStatus = model.ExtStatus,
                    ExtIdAreaDeptoNavigation = null,
                };

                await db.MceCatExtensiones.AddAsync(oExtension);
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
        public async Task<IActionResult> EditData(ExtensionViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatExtension? oExtension = await db.MceCatExtensiones.FindAsync(model.IdExtension);

                if (oExtension != null)
                {
                    oExtension.ExtNoExtension = model.ExtNoExtension;
                    oExtension.ExtIdAreaDepto = model.ExtIdAreaDepto;
                    oExtension.ExtStatus = model.ExtStatus;

                    db.Entry(oExtension).State = EntityState.Modified;
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

                MceCatExtension? oExtension = await db.MceCatExtensiones.FindAsync(id);
                //db.Remove(oPersona);

                if (oExtension != null)
                {
                    oExtension.ExtStatus = isActivate;
                    db.Entry(oExtension).State = EntityState.Modified;
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
