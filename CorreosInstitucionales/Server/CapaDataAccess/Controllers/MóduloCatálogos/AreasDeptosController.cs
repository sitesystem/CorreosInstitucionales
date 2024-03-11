using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AreasDeptosController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatAreasDepto>> oResponse = new();

            try
            {
                var list = new List<McCatAreasDepto>();

                if (filterByStatus)
                    list = await _db.McCatAreasDeptos.Where(a => a.AreStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatAreasDeptos.ToListAsync();

                if (list == null)
                    return BadRequest(oResponse);

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
            Response<McCatAreasDepto> oResponse = new();

            try
            {
                var item = await _db.McCatAreasDeptos.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpGet("filterByExtension/{extension}")]
        public async Task<IActionResult> GetDataByExtension(string extension)
        {
            Response<McCatAreasDepto?> oResponse = new();

            try
            {
                McCatAreasDepto? result = await _db.McCatAreasDeptos.FirstOrDefaultAsync
                (
                    a =>
                        a.AreNoExtension != null &&
                        a.AreNoExtension.Equals(extension)
                );

                oResponse.Success = 1;
                oResponse.Data = result;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(RequestViewModel_AreaDepto model)
        {
            Response<object> oResponse = new();
            
            try
            {
                McCatAreasDepto oAreaDepto = new()
                {
                    IdAreaDepto = model.IdAreaDepto,
                    AreNombreAreaDepto = model.AreNombreAreaDepto,
                    AreNoExtension = model.AreNoExtension,
                    AreTitular = model.AreTitular,
                    AreIdEdificio = model.AreIdEdificio,
                    AreIdPiso = model.AreIdPiso,
                    AreStatus = true, // model.AreStatus
                };

                await _db.McCatAreasDeptos.AddAsync(oAreaDepto);
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
        public async Task<IActionResult> EditData(RequestViewModel_AreaDepto model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatAreasDepto? oAreaDepto = await _db.McCatAreasDeptos.FindAsync(model.IdAreaDepto);

                if (oAreaDepto != null)
                {
                    oAreaDepto.AreNombreAreaDepto = model.AreNombreAreaDepto;
                    oAreaDepto.AreNoExtension = model.AreNoExtension;
                    oAreaDepto.AreTitular = model.AreTitular;
                    oAreaDepto.AreIdEdificio = model.AreIdEdificio;
                    oAreaDepto.AreIdPiso = model.AreIdPiso;
                    oAreaDepto.AreStatus = model.AreStatus;

                    _db.Entry(oAreaDepto).State = EntityState.Modified;
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

        //[HttpDelete("{Id}")]
        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatAreasDepto? oAreaDepto = await _db.McCatAreasDeptos.FindAsync(id);
                
                if (oAreaDepto != null)
                {
                    oAreaDepto.AreStatus = isActivate;
                    _db.Entry(oAreaDepto).State = EntityState.Modified;
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
