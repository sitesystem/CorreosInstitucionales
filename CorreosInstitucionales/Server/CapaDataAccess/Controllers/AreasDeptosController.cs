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
    public class AreasDeptosController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatAreasDepto>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceCatAreasDepto>();

                if (filterByStatus)
                    list = await db.MceCatAreasDeptos
                                   .Where(a => a.AreStatus.Equals(filterByStatus))
                                   .Include(a => a.AreIdEdificioNavigation)
                                   .Include(a => a.AreIdPisoNavigation)
                                   .ToListAsync();
                else
                    list = await db.MceCatAreasDeptos
                                   .Include(a => a.AreIdEdificioNavigation)
                                   .Include(a => a.AreIdPisoNavigation)
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
            Response<MceCatAreasDepto> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceCatAreasDeptos
                                .Include(a => a.AreIdEdificioNavigation)
                                .Include(a => a.AreIdPisoNavigation)
                                .FirstOrDefaultAsync(a => a.IdAreaDepto == id);
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
        public async Task<IActionResult> AddData(AreaDeptoViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatAreasDepto oÁreaDepto = new()
                {
                    IdAreaDepto = model.IdAreaDepto,
                    AreNombre = model.AreNombre,
                    AreTitular = model.AreTitular,
                    AreIdEdificio = model.AreIdEdificio,
                    AreIdPiso = model.AreIdPiso,
                    AreStatus = true,
                    AreIdEdificioNavigation = null,
                    AreIdPisoNavigation = null,
                };
                await db.MceCatAreasDeptos.AddAsync(oÁreaDepto);
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
        public async Task<IActionResult> EditData(AreaDeptoViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatAreasDepto? oÁreaDepto = await db.MceCatAreasDeptos.FindAsync(model.IdAreaDepto);

                if (oÁreaDepto != null)
                {
                    oÁreaDepto.AreNombre = model.AreNombre;
                    oÁreaDepto.AreTitular = model.AreTitular;
                    oÁreaDepto.AreIdEdificio = model.AreIdEdificio;
                    oÁreaDepto.AreIdPiso = model.AreIdPiso;
                    oÁreaDepto.AreStatus = model.AreStatus;
                    db.Entry(oÁreaDepto).State = EntityState.Modified;
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

        //[HttpDelete("{Id}")]
        [HttpPut("editByIdStatus/{Id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatAreasDepto? oÁreaDepto = await db.MceCatAreasDeptos.FindAsync(id);

                if (oÁreaDepto != null)
                {
                    oÁreaDepto.AreStatus = isActivate;
                    db.Entry(oÁreaDepto).State = EntityState.Modified;
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
