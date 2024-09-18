using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SemestresController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatSemestre>> oResponse = new();

            try
            {
                var list = new List<McCatSemestre>();

                if (filterByStatus)
                    list = await _db.McCatSemestres.ToListAsync();

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
            Response<McCatAnuncio> oResponse = new();

            try
            {
                var item = await _db.McCatAnuncios.FindAsync(id);
                oResponse.Success = 1;
                oResponse.Data = item;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPost]
        public async Task<IActionResult> AddData(RequestViewModel_Semestre model)
        {
            Response<object> oResponse = new();

            try
            {
                McCatSemestre oSemestre = new()
                {
                    IdSemestre = model.IdSemestre,
                    SemNombre = model.SemNombre
                };

                await _db.McCatSemestres.AddAsync(oSemestre);
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
        public async Task<IActionResult> EditData(RequestViewModel_Semestre model)
        {
            Response<object> oRespuesta = new();

            try
            {
                McCatSemestre? oSemestre = await _db.McCatSemestres.FindAsync(model.IdSemestre);

                if (oSemestre != null)
                {
                    oSemestre.SemNombre = model.SemNombre;

                    _db.Entry(oSemestre).State = EntityState.Modified;
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
