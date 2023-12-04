using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Server.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CarrerasController : Controller
    {
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<MceCatCarrera>> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = new List<MceCatCarrera>();

                if (filterByStatus)
                    list = await db.MceCatCarreras.Where(c => c.CarrStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await db.MceCatCarreras.ToListAsync();

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
            Response<MceCatCarrera> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();
                var list = await db.MceCatCarreras.FindAsync(id);
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
        public async Task<IActionResult> AddData(CarreraViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatCarrera oCarrera = new()
                {
                    IdCarrera = model.IdCarrera,
                    CarrClave = model.CarrClave,
                    CarrNombre = model.CarrNombre,
                    CarrStatus = true
                };

                await db.MceCatCarreras.AddAsync(oCarrera);
                await db.SaveChangesAsync();

                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(CarreraViewModel model)
        {
            Response<object> oRespuesta = new();

            try
            {
                using DbCorreosInstUpiicsaContext db = new();

                MceCatCarrera? oCarrera = await db.MceCatCarreras.FindAsync(model.IdCarrera);

                if (oCarrera != null)
                {
                    oCarrera.CarrClave = model.CarrClave;
                    oCarrera.CarrNombre = model.CarrNombre;
                    oCarrera.CarrStatus = model.CarrStatus;

                    db.Entry(oCarrera).State = EntityState.Modified;
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

                MceCatCarrera? oCarrera = await db.MceCatCarreras.FindAsync(id);

                //db.Remove(oPersona);
                if (oCarrera != null)
                {
                    oCarrera.CarrStatus = isActivate;
                    db.Entry(oCarrera).State = EntityState.Modified;
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
