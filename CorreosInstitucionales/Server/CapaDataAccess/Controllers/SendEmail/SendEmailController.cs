using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

using CorreosInstitucionales.Shared.CapaTools;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class SendEmailController(IConfiguration config) : ControllerBase
    {
        private readonly IConfiguration _config = config;

        [HttpPost]
        public async Task<IActionResult> SendEmail(RequestDTO_SendEmail model)
        {
            Response<object> oResponse = new() { Success = 0 };

            if (model.EmailTo.ToLower().EndsWith("@localhost"))
            {
                oResponse.Data = "Correo de prueba";
                return Ok(oResponse);
            }

            try
            {
                string host = _config.GetSection("Email:Host").Value ?? string.Empty;
                int port = Convert.ToInt32(_config.GetSection("Email:Port").Value ?? string.Empty);
                string emailFrom = _config.GetSection("Email:UserName").Value ?? string.Empty;
                string password = _config.GetSection("Email:PassWord").Value ?? string.Empty;

                using MailMessage mailMessage = new(emailFrom, model.EmailTo, model.Subject, model.Body)
                {
                    IsBodyHtml = true
                };

                using SmtpClient smtpClient = new(host)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Port = port,
                    Credentials = new NetworkCredential(emailFrom, password)
                };

                await smtpClient.SendMailAsync(mailMessage);
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
