using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Client.CapaPresentation_ComponentsPages_UI_UX.MóduloCatálogos;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AreasDeptosController(DbCorreosInstitucionalesUpiicsaContext _db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = _db;

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

        [HttpPost]
        public async Task<IActionResult> AddData(RequestViewModel_AreaDepto model)
        {
            Response<object> oResponse = new();
            McCatAreasDepto item = new McCatAreasDepto()
            {
                AreIdEdificio = model.AreIdEdificio,
                AreIdPiso = model.AreIdPiso,
                AreNombreAreaDepto = model.AreNombreAreaDepto,
                AreNoExtension = model.AreNoExtension,
                AreTitular = model.AreTitular,
                AreStatus = true
            };

            try
            {
                model.AreStatus = true;

                await _db.McCatAreasDeptos.AddAsync(item);
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
                McCatAreasDepto? item = await _db.McCatAreasDeptos.FindAsync(model.IdAreaDepto);

                if (item != null)
                {
                    item.AreIdEdificio = model.AreIdEdificio;
                    item.AreIdPiso = model.AreIdPiso;
                    item.AreNombreAreaDepto = model.AreNombreAreaDepto;
                    item.AreNoExtension = model.AreNoExtension;
                    item.AreTitular = model.AreTitular;
                    item.AreStatus = true;

                    _db.Entry(item).State = EntityState.Modified;
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
                McCatAreasDepto? item = await _db.McCatAreasDeptos.FindAsync(id);
                
                if(item != null)
                {
                    item.AreStatus = isActivate;

                    _db.Entry(item).State = EntityState.Modified;
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
