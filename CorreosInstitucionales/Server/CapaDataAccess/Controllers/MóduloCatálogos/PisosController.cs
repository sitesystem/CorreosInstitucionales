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
    public class PisosController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<McCatPiso>> oResponse = new();

            try
            {
                var list = new List<McCatPiso>();

                if (filterByStatus)
                    list = await _db.McCatPisos.Where(p => p.PisoStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatPisos.ToListAsync();

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
            Response<McCatPiso> oResponse = new();

            try
            {
                var list = await _db.McCatPisos.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_Piso model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatPiso oPiso = new()
                {
                    IdPiso = model.IdPiso,
                    PisoDescripcion = model.PisoDescripcion,
                    PisoStatus = true
                };

                await _db.McCatPisos.AddAsync(oPiso);
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
        public async Task<IActionResult> EditData(RequestViewModel_Piso model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatPiso? oPiso = await _db.McCatPisos.FindAsync(model.IdPiso);

                if (oPiso != null)
                {
                    oPiso.PisoDescripcion = model.PisoDescripcion;
                    oPiso.PisoStatus = model.PisoStatus;

                    _db.Entry(oPiso).State = EntityState.Modified;
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
                McCatPiso? oPiso = await _db.McCatPisos.FindAsync(id);
                //db.Remove(oPersona);

                if (oPiso != null)
                {
                    oPiso.PisoStatus = isActivate;
                    _db.Entry(oPiso).State = EntityState.Modified;
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
