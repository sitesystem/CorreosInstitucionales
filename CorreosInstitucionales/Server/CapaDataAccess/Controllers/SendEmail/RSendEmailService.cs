using System.Net.Mail;
using System.Net;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers.SendEmail
{
    public class RSendEmailService(IConfiguration config) : ISendEmailService
    {
        private readonly IConfiguration _config = config;

        public async Task SendEmail(RequestDTO_SendEmail modelEmailTo)
        {
			try
			{
				string host = _config.GetSection("Email:Host").Value ?? string.Empty;
				int port = Convert.ToInt32(_config.GetSection("Email:Port").Value ?? string.Empty);
				string emailFrom = _config.GetSection("Email:UserName").Value ?? string.Empty;
				string password = _config.GetSection("Email:PassWord").Value ?? string.Empty;

				using MailMessage mailMessage = new(emailFrom, modelEmailTo.EmailTo, modelEmailTo.Subject, modelEmailTo.Body)
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
				// Manejar la excepción según tus necesidades (puedes loggearla, lanzarla nuevamente, etc.).
				Console.WriteLine($"Error al enviar el correo electrónico: {ex.Message}");
				throw;
			}
        }
    }
}
