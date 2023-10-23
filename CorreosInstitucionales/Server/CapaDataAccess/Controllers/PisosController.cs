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
    public class PisosController : ControllerBase
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatPiso>> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = new List<MceCatPiso>();

                    if (filterByStatus)
                        list = await db.MceCatPisos.Where(e => e.PisoStatus.Equals(filterByStatus)).ToListAsync();
                    else
                        list = await db.MceCatPisos.ToListAsync();

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
            Response<MceCatPiso> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    var list = await db.MceCatPisos.FindAsync(id);
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
        public async Task<IActionResult> AddData(PisoViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using (DbCorreosInstUpiicsaContext db = new())
                {
                    MceCatPiso oPiso = new()
                    {
                        IdPiso = model.IdPiso,
                        PisoDescripcion = model.PisoDescripcion,
                        PisoStatus = true
                    };
                    await db.MceCatPisos.AddAsync(oPiso);
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
        public IActionResult EditData(PisoViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                MceCatPiso? oPiso = db.MceCatPisos.Find(model.IdPiso);
                if (oPiso != null)
                {
                    oPiso.PisoDescripcion = model.PisoDescripcion;                   
                    oPiso.PisoStatus = model.PisoStatus;

                    db.Entry(oPiso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                MceCatPiso? oPiso = db.MceCatPisos.Find(id);
                //db.Remove(oPersona);
                if (oPiso != null)
                {
                    oPiso.PisoStatus = isActivate;
                    db.Entry(oPiso).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
