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
    public class EdificiosController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<McCatEdificio>> oResponse = new();

            try
            {
                var list = new List<McCatEdificio>();

                if (filterByStatus)
                    list = await _db.McCatEdificios.Where(e => e.EdiStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatEdificios.ToListAsync();

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
            Response<McCatEdificio> oResponse = new();

            try
            {
                var list = await _db.McCatEdificios.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_Edificio model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatEdificio oEdificio = new()
                {
                    IdEdificio = model.IdEdificio,
                    EdiNombreOficial = model.EdiNombreOficial,
                    EdiNombreAlias = model.EdiNombreAlias,
                    EdiStatus = true
                };

                await _db.McCatEdificios.AddAsync(oEdificio);
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
        public async Task<IActionResult> EditData(RequestViewModel_Edificio model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatEdificio? oEdificio = await _db.McCatEdificios.FindAsync(model.IdEdificio);

                if (oEdificio != null)
                {
                    oEdificio.EdiNombreOficial = model.EdiNombreOficial;
                    oEdificio.EdiNombreAlias = model.EdiNombreAlias;
                    oEdificio.EdiStatus = model.EdiStatus;

                    _db.Entry(oEdificio).State = EntityState.Modified;
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
                McCatEdificio? oEdificio = await _db.McCatEdificios.FindAsync(id);
                //db.Remove(oPersona);

                if (oEdificio != null)
                {
                    oEdificio.EdiStatus = isActivate;
                    _db.Entry(oEdificio).State = EntityState.Modified;
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
