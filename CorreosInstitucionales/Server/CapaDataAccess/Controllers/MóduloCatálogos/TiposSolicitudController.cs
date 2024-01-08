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
    public class TiposSolicitudController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<McCatTiposSolicitud>> oResponse = new();

            try
            {
                var list = new List<McCatTiposSolicitud>();

                if (filterByStatus)
                    list = await _db.McCatTiposSolicituds.Where(ts => ts.TiposolStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatTiposSolicituds.ToListAsync();

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
            Response<McCatTiposSolicitud> oResponse = new();

            try
            {
                var list = await _db.McCatTiposSolicituds.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_TipoSolicitud model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatTiposSolicitud oTipoSolicitud = new()
                {
                    IdTipoSolicitud = model.IdTipoSolicitud,
                    TiposolDescripcion = model.TiposolDescripcion,
                    TiposolStatus = true
                };

                await _db.McCatTiposSolicituds.AddAsync(oTipoSolicitud);
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
        public async Task<IActionResult> EditData(RequestViewModel_TipoSolicitud model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatTiposSolicitud? oTipoSolicitud = await _db.McCatTiposSolicituds.FindAsync(model.IdTipoSolicitud);

                if (oTipoSolicitud != null)
                {
                    oTipoSolicitud.TiposolDescripcion = model.TiposolDescripcion;
                    oTipoSolicitud.TiposolStatus = model.TiposolStatus;

                    _db.Entry(oTipoSolicitud).State = EntityState.Modified;
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
                McCatTiposSolicitud? oTipoSolicitud = await _db.McCatTiposSolicituds.FindAsync(id);
                //db.Remove(oPersona);

                if (oTipoSolicitud != null)
                {
                    oTipoSolicitud.TiposolStatus = isActivate;
                    _db.Entry(oTipoSolicitud).State = EntityState.Modified;
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
