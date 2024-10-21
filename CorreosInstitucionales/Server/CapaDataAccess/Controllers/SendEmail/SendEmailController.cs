using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class SendEmailController(ISendEmailService servicioEmail) : ControllerBase
    {
        private readonly ISendEmailService _servicioEmail = servicioEmail;

        [HttpPost]
        public async Task<IActionResult> SendEmail(RequestDTO_SendEmail model)
        {
            Response<object> oResponse = new();

            try
            {
                await _servicioEmail.SendEmail(model);
                oResponse.Success = 1;
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message;
            }

            return Ok(oResponse);
        }
    }
}
