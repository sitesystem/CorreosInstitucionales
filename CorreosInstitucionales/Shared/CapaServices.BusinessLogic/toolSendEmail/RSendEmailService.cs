﻿using CorreosInstitucionales.Shared.CapaDataAccess;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail
{
    public class RSendEmailService
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _emailFrom;
        private readonly string _password;

        public RSendEmailService(IConfiguration config)
        {
            _host = config.GetSection("Email:Host").Value ?? string.Empty;
            _port = Convert.ToInt32(config.GetSection("Email:Port").Value ?? string.Empty);
            _emailFrom = config.GetSection("Email:UserName").Value ?? string.Empty;
            _password = config.GetSection("Email:PassWord").Value ?? string.Empty;
        }

        public async Task<Response<string>> SendManyAsync
            (
                IEnumerable<RequestDTO_SendEmail> oList,
                IDictionary<string, byte[]>? attachments = null
            )
        {
            Response<string> oResponse = new Response<string>() { Success = 1 };

            try
            {
                using SmtpClient smtpClient = new(_host)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Port = _port,
                    Credentials = new NetworkCredential(_emailFrom, _password)
                };

                foreach(RequestDTO_SendEmail oSendEmail in oList)
                {
                    oResponse.Data += await SendSingleMail(smtpClient, oSendEmail, attachments);
                }
            }
            catch (Exception ex)
            {
                oResponse.Data = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return oResponse;
        }

        public async Task<string?> SendSingleMail
            (
                SmtpClient smtpClient,
                RequestDTO_SendEmail oSendEmail,
                IDictionary<string, byte[]>? attachments = null
            )
        {
            if (oSendEmail.EmailTo.ToLower().EndsWith("@localhost"))
            {
                return "Correo de prueba\n\r";
            }

            using MailMessage mailMessage = new(_emailFrom, oSendEmail.EmailTo, oSendEmail.Subject, null)
            {
                IsBodyHtml = true
            };

            LinkedResource logo = new LinkedResource(
                new MemoryStream(AppCache.LogoSACI) { Position = 0 },
                new System.Net.Mime.ContentType("image/png"));

            string body = (oSendEmail.Body??"").Replace("{logo_saci}",$"cid:{logo.ContentId}");

            AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, System.Net.Mime.MediaTypeNames.Text.Html);

            av.LinkedResources.Add(logo);

            if (attachments is not null)
            {
                foreach( KeyValuePair<string,byte[]> attachment in attachments )
                {
                    mailMessage.Attachments.Add(new Attachment(new MemoryStream(attachment.Value), attachment.Key));
                }
            }

            mailMessage.AlternateViews.Add(av);

            await smtpClient.SendMailAsync(mailMessage);
            return null;
        }

        public async Task<Response<string>> SendEmailAsync
            (   
                RequestDTO_SendEmail oSendEmail,
                IDictionary<string, byte[]>? attachments = null
            )
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
                using SmtpClient smtpClient = new(_host)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Port = _port,
                    Credentials = new NetworkCredential(_emailFrom, _password)
                };

                oResponse.Data = await SendSingleMail(smtpClient, oSendEmail, attachments);
                
            }
            catch (Exception ex)
            {
                oResponse.Data = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return oResponse;
        }
    }
}
