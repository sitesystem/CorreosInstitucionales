using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.Extensions.Configuration;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail
{
    public class RSendEmailService(IConfiguration config)
    {
        IConfiguration _config = config;

        public async Task<Response<string>> SendEmailAsync(RequestDTO_SendEmail oSendEmail)
        {
            Response<string> oResponse = new Response<string>() { Success = 0 };

            if (oSendEmail.EmailTo.ToLower().EndsWith("@localhost"))
            {
                oResponse.Success = 1;
                oResponse.Data = "Correo de prueba";
                return oResponse;
            }

            try
            {
                string host = _config.GetSection("Email:Host").Value ?? string.Empty;
                int port = Convert.ToInt32(_config.GetSection("Email:Port").Value ?? string.Empty);
                string emailFrom = _config.GetSection("Email:UserName").Value ?? string.Empty;
                string password = _config.GetSection("Email:PassWord").Value ?? string.Empty;

                using MailMessage mailMessage = new(emailFrom, oSendEmail.EmailTo, oSendEmail.Subject, oSendEmail.Body)
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
            }
            catch (Exception ex)
            {
                oResponse.Data = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return oResponse;
        }
    }
}
