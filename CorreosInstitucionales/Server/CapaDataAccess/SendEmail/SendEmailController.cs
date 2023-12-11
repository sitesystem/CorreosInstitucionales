using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.SendEmail
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SendEmailController : ControllerBase
    {
        private readonly ISendEmail _servicioEmail;

        public SendEmailController(ISendEmail servicioEmail)
        {
            _servicioEmail = servicioEmail;
        }

        [HttpPost]
        public IActionResult SendEmail(SendEmailViewModel model)
        {
            Response<object> oResponse = new();

            try
            {
                _servicioEmail.SendEmail(model);
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
