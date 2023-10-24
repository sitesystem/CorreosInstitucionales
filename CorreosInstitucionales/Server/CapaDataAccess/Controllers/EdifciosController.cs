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
    public class EdificiosController : Controller
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatEdificio>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceCatEdificio>();

                    if (filterByStatus)
                        list = await db.MceCatEdificios.Where(e => e.EdiStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceCatEdificios.ToListAsync();

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
            Response<MceCatEdificio> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatEdificios.FindAsync(id);
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
        public async Task<IActionResult> AddData(EdificioViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatEdificio oEdificio = new()
                    {
                        IdEdificio = model.IdEdificio,
                        EdiNombreOficial = model.EdiNombreOficial,
                        EdiNombreAlias = model.EdiNombreAlias,
                        EdiStatus = true
                    };
                    await db.MceCatEdificios.AddAsync(oEdificio);
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
        public IActionResult EditData(EdificioViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatEdificio? oEdificio = db.MceCatEdificios.Find(model.IdEdificio);
                if (oEdificio != null)
                {
                    oEdificio.EdiNombreOficial = model.EdiNombreOficial;
                    oEdificio.EdiNombreAlias = model.EdiNombreAlias;
                    oEdificio.EdiStatus = model.EdiStatus;

                    db.Entry(oEdificio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
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
        public IActionResult EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatEdificio? oEdificio = db.MceCatEdificios.Find(id);
                //db.Remove(oPersona);
                if (oEdificio != null)
                {
                    oEdificio.EdiStatus = isActivate;
                    db.Entry(oEdificio).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
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
