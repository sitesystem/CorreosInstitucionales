using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinksController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatLink>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceCatLink>();

                    if (filterByStatus)
                        list = await db.MceCatLinks.Where(e => e.LinkStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceCatLinks.ToListAsync();

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
            Response<MceCatLink> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatLinks.FindAsync(id);
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
        public async Task<IActionResult> AddData(LinkViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatLink oLink = new()
                    {
                        IdLink = model.IdLink,
                        LinkNombre = model.LinkNombre,
                        LinkStatus = true
                    };
                    await db.MceCatLinks.AddAsync(oLink);
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
        public async Task<IActionResult> EditData(LinkViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatLink? oLink = await db.MceCatLinks.FindAsync(model.IdLink);
                if (oLink != null)
                {
                    oLink.LinkNombre = model.LinkNombre;
                    oLink.LinkStatus = model.LinkStatus;

                    db.Entry(oLink).State = EntityState.Modified;
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
                MceCatLink? oLink = db.MceCatLinks.Find(id);
                //db.Remove(oPersona);
                if (oLink != null)
                {
                    oLink.LinkStatus = isActivate;
                    db.Entry(oLink).State = EntityState.Modified;
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
