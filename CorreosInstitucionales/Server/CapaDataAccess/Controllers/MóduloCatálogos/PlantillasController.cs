using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]

    public class PlantillasController(DbCorreosInstitucionalesUpiicsaContext db) : ControllerBase
    {
        private readonly DbCorreosInstitucionalesUpiicsaContext _db = db;

        [HttpGet("filter/{filter}")]
        public async Task<IActionResult> GetAllDataByFilter(int filter)
        {
            Response<List<McCatPlantillas>> oResponse = new();

            try
            {
                List<McCatPlantillas> list = await _db.McCatPlantillas.Where(a => a.PlaFiltro.Equals(filter)).ToListAsync();
                
                oResponse.Success = 1;
                oResponse.Data = list;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }
            
        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllDataByStatus(bool filterByStatus)
        {
            Response<List<McCatPlantillas>> oResponse = new();

            try
            {
                var list = new List<McCatPlantillas>();

                if (filterByStatus)
                    list = await _db.McCatPlantillas.Where(a => a.PlaStatus.Equals(filterByStatus)).ToListAsync();
                else
                    list = await _db.McCatPlantillas.ToListAsync();

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
            Response<McCatPlantillas> oResponse = new();

            try
            {
                var item = await _db.McCatPlantillas.FindAsync(id);
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
        public async Task<IActionResult> AddData(RequestViewModel_Plantilla model)
        {
            Response<object> oResponse = new();

            try
            {
                await _db.McCatPlantillas.AddAsync(model);
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
        public async Task<IActionResult> EditData(RequestViewModel_Plantilla model)
        {
            Response<object> oRespuesta = new();

            try
            {
                _db.Entry(model).State = EntityState.Modified;
                await _db.SaveChangesAsync();

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
                McCatPlantillas? entry = await _db.McCatPlantillas.FindAsync(id);
                //db.Remove(oAnuncio);

                if (entry != null)
                {
                    entry.PlaStatus = isActivate;
                    _db.Entry(entry).State = EntityState.Modified;
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
