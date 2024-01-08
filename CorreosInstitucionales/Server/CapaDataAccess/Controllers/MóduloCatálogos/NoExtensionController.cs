using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class NoExtensionController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<McCatNoExtension>> oResponse = new();

            try
            {
                var list = new List<McCatNoExtension>();

                if (filterByStatus)
                    list = await _db.McCatNoExtensions
                                    .Where(e => e.ExtStatus.Equals(filterByStatus))
                                    .Include(e => e.ExtIdAreaDeptoNavigation)
                                    .ThenInclude(a => a.AreIdEdificioNavigation)
                                    .Include(e => e.ExtIdAreaDeptoNavigation)
                                    .ThenInclude(a => a.AreIdPisoNavigation)
                                    .ToListAsync();
                else
                    list = await _db.McCatNoExtensions
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
            Response<McCatNoExtension> oResponse = new();

            try
            {
                var list = await _db.McCatNoExtensions
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
        public async Task<IActionResult> AddData(RequestViewModel_NoExtension model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatNoExtension oExtension = new()
                {
                    IdExtension = model.IdExtension,
                    ExtNoExtension = model.ExtNoExtension,
                    ExtIdAreaDepto = model.ExtIdAreaDepto,
                    ExtStatus = model.ExtStatus,
                    ExtIdAreaDeptoNavigation = null,
                };

                await _db.McCatNoExtensions.AddAsync(oExtension);
                await _db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(RequestViewModel_NoExtension model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatNoExtension? oExtension = await _db.McCatNoExtensions.FindAsync(model.IdExtension);

                if (oExtension != null)
                {
                    oExtension.ExtNoExtension = model.ExtNoExtension;
                    oExtension.ExtIdAreaDepto = model.ExtIdAreaDepto;
                    oExtension.ExtStatus = model.ExtStatus;

                    _db.Entry(oExtension).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
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
                McCatNoExtension? oExtension = await _db.McCatNoExtensions.FindAsync(id);
                //db.Remove(oPersona);

                if (oExtension != null)
                {
                    oExtension.ExtStatus = isActivate;
                    _db.Entry(oExtension).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
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
