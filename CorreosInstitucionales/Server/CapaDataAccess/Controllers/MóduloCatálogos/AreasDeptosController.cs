using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.MóduloCatálogos
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AreasDeptosController(DbCorreosInstitucionalesUpiicsaContext _db) : ControllerBase
    {
        private readonly IGenericService<McCatAreasDepto, RequestViewModel_AreaDepto> _areaDeptoService = areaDeptoService;

        [HttpGet("filterByStatus/{filterByStatus}")]
        public async Task<IActionResult> GetAllData(bool filterByStatus)
        {
            Response<List<McCatAreasDepto>> oResponse = new();

            try
            {
                var list = await _areaDeptoService.GetAllData(filterByStatus);

                if (list == null)
                    return BadRequest(oResponse);

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
            Response<McCatAreasDepto> oResponse = new();

            try
            {
                var item = await _areaDeptoService.GetDataById(id);

                if (item == null)
                    return BadRequest(oResponse);

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
        public async Task<IActionResult> AddData(RequestViewModel_AreaDepto model)
        {
            Response<object> oResponse = new();

            try
            {
                await _areaDeptoService.AddData(model);
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }

        [HttpPut]
        public async Task<IActionResult> EditData(RequestViewModel_AreaDepto model)
        {
            Response<object> oRespuesta = new();

            try
            {
                await _areaDeptoService.EditData(model);
                oRespuesta.Success = 1;
            }
            catch (Exception ex)
            {
                oRespuesta.Message = ex.Message;
            }

            return Ok(oRespuesta);
        }

        //[HttpDelete("{Id}")]
        [HttpPut("editByIdStatus/{id}/{isActivate}")]
        public async Task<IActionResult> EnableDisableDataById(int id, bool isActivate)
        {
            Response<object> oRespuesta = new();

            try
            {
                await _areaDeptoService.EnableDisableDataById(id, isActivate);
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
