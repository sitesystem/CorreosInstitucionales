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
    public class EscuelasController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatEscuela>> oResponse = new();

            try
            {
                var list = new List<McCatEscuela>();

                if (filterByStatus)
                    list = await _db.McCatEscuelas.Where(e => e.EscStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatEscuelas.ToListAsync();

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
            Response<McCatEscuela> oResponse = new();

            try
            {
                var list = await _db.McCatEscuelas.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_Escuela model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatEscuela oEscuela = new()
                {
                    IdEscuela = model.IdEscuela,
                    EscNoEscuela = model.EscNoEscuela,
                    EscNombreLargo = model.EscNombreLargo,
                    EscNombreCorto = model.EscNombreCorto,
                    EscLogo = model.EscLogo,
                    EscStatus = true
                };

                await _db.McCatEscuelas.AddAsync(oEscuela);
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
        public async Task<IActionResult> EditData(RequestViewModel_Escuela model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatEscuela? oEscuela = await _db.McCatEscuelas.FindAsync(model.IdEscuela);

                if (oEscuela != null)
                {
                    oEscuela.EscNoEscuela = model.EscNoEscuela;
                    oEscuela.EscNombreLargo = model.EscNombreLargo;
                    oEscuela.EscNombreCorto = model.EscNombreCorto;
                    oEscuela.EscLogo = model.EscLogo;
                    oEscuela.EscStatus = model.EscStatus;

                    _db.Entry(oEscuela).State = EntityState.Modified;
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
                McCatEscuela? oEscuela = await _db.McCatEscuelas.FindAsync(id);
                //db.Remove(oPersona);

                if (oEscuela != null)
                {
                    oEscuela.EscStatus = isActivate;
                    _db.Entry(oEscuela).State = EntityState.Modified;
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
