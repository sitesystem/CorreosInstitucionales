using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.CorreoService
{
    public class REmail : IEmail
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "correo.I.pn666@gmail.com";
            var pw = "contrasena01";

            var client = new SmtpClient("smtp-mail.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail,pw)
            };
            return client.SendMailAsync(
                new MailMessage(from: mail,
                to: email, 
                subject, 
                message));
        }
    }
}
