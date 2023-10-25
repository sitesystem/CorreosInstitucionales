using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposSolicitudController : Controller
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatTipoSolicitud>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceCatTipoSolicitud>();

                    if (filterByStatus)
                        list = await db.MceCatTipoSolicituds.Where(e => e.TiposolStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceCatTipoSolicituds.ToListAsync();

                    oResponse.Success = 1;
                    oResponse.Data = list;
                }
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
            Response<MceCatTipoSolicitud> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatTipoSolicituds.FindAsync(id);
                    oResponse.Success = 1;
                    oResponse.Data = list;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(TipoSolicitudViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatTipoSolicitud oTipoSolicitud = new()
                    {
                        IdTipoSolicitud = model.IdTipoSolicitud,
                        TiposolDescripcion = model.TiposolDescripcion,
                        TiposolStatus = model.TiposolStatus
                    };
                    await db.MceCatTipoSolicituds.AddAsync(oTipoSolicitud);
                    await db.SaveChangesAsync();

                    oResponse.Success = 1;
                }
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(TipoSolicitudViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatTipoSolicitud? oTipoSolicitud = db.MceCatTipoSolicituds.Find(model.IdTipoSolicitud);
                if (oTipoSolicitud != null)
                {
                    oTipoSolicitud.TiposolDescripcion = model.TiposolDescripcion;
                    oTipoSolicitud.TiposolStatus = model.TiposolStatus;

                    db.Entry(oTipoSolicitud).State = EntityState.Modified;
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
                MceCatTipoSolicitud? oTipoSolicitud = db.MceCatTipoSolicituds.Find(id);
                //db.Remove(oPersona);
                if (oTipoSolicitud != null)
                {
                    oTipoSolicitud.TiposolStatus = isActivate;
                    db.Entry(oTipoSolicitud).State = EntityState.Modified;
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
