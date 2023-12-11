using System.Net;
using System.Net.Mail;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.SendEmail
{
    public class RSendEmail : ISendEmail
    {
        private readonly IConfiguration _config;

        public RSendEmail(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(SendEmailViewModel model)
        {
            string? host = _config.GetSection("Email:Host").Value;
            int port = Convert.ToInt32(_config.GetSection("Email:Port").Value);
            string? emailFrom = _config.GetSection("Email:UserName").Value;
            string? password = _config.GetSection("Email:PassWord").Value;

            MailMessage mailMessage = new(emailFrom, model.EmailTo, model.Subject, model.Body)
            {
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new(host)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Port = port,
                Credentials = new NetworkCredential(emailFrom, password)
            };

            smtpClient.Send(mailMessage);
            smtpClient.Dispose();
        }
    }
}
