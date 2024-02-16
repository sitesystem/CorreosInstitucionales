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
    public class LinksController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatLink>> oResponse = new();

            try
            {
                var list = new List<McCatLink>();

                if (filterByStatus)
                    list = await _db.McCatLinks.Where(l => l.LinkStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatLinks.ToListAsync();

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
            Response<McCatLink> oResponse = new();

            try
            {
                var list = await _db.McCatLinks.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }
            return Ok(oResponse);
        }

        [HttpGet("filterByNombre/{name}")]
        public async Task<IActionResult> GetDataByNombre(string name)
        {
            Response<McCatLink> oResponse = new();

            try
            {
                var list = await _db.McCatLinks.Where(l => l.LinkNombre == name).FirstOrDefaultAsync();
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
        public async Task<IActionResult> AddData(RequestViewModel_Link model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatLink oLink = new()
                {
                    IdLink = model.IdLink,
                    LinkNombre = model.LinkNombre,
                    LinkEnlace = model.LinkEnlace,
                    LinkStatus = true
                };

                await _db.McCatLinks.AddAsync(oLink);
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
        public async Task<IActionResult> EditData(RequestViewModel_Link model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatLink? oLink = await _db.McCatLinks.FindAsync(model.IdLink);

                if (oLink != null)
                {
                    // oLink.LinkNombre = model.LinkNombre; Deja comentado para no editar el nombre
                    oLink.LinkEnlace = model.LinkEnlace;
                    oLink.LinkStatus = model.LinkStatus;

                    _db.Entry(oLink).State = EntityState.Modified;
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
                McCatLink? oLink = await _db.McCatLinks.FindAsync(id);
                //db.Remove(oPersona);

                if (oLink != null)
                {
                    oLink.LinkStatus = isActivate;
                    _db.Entry(oLink).State = EntityState.Modified;
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
