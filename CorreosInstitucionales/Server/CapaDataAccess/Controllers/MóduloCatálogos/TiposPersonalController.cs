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
    public class TiposPersonalController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatTiposPersonal>> oResponse = new();

            try
            {
                var list = new List<McCatTiposPersonal>();

                if (filterByStatus)
                    list = await _db.McCatTiposPersonals.Where(tp => tp.TipoperStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatTiposPersonals.ToListAsync();

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
            Response<McCatTiposPersonal> oResponse = new();

            try
            {
                var list = await _db.McCatTiposPersonals.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_TipoPersonal model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatTiposPersonal oTipoPersonal = new()
                {
                    IdTipoPersonal = model.IdTipoPersonal,
                    TipoperNombre = model.TipoperNombre,
                    TipoperDescripcion = model.TipoperDescripcion,
                    TipoperStatus = true
                };

                await _db.McCatTiposPersonals.AddAsync(oTipoPersonal);
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
        public async Task<IActionResult> EditData(RequestViewModel_TipoPersonal model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatTiposPersonal? oTipoPersonal = await _db.McCatTiposPersonals.FindAsync(model.IdTipoPersonal);

                if (oTipoPersonal != null)
                {
                    oTipoPersonal.TipoperNombre = model.TipoperNombre;
                    oTipoPersonal.TipoperDescripcion = model.TipoperDescripcion;
                    oTipoPersonal.TipoperStatus = model.TipoperStatus;

                    _db.Entry(oTipoPersonal).State = EntityState.Modified;
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
                McCatTiposPersonal? oTipoPersonal = await _db.McCatTiposPersonals.FindAsync(id);
                //db.Remove(oPersona);

                if (oTipoPersonal != null)
                {
                    oTipoPersonal.TipoperStatus = isActivate;
                    _db.Entry(oTipoPersonal).State = EntityState.Modified;
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
