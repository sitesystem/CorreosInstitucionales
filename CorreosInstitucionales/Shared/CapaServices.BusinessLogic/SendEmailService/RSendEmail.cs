using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.SendEmailService
{
    public class RSendEmail : ISendEmail
    {
        private readonly HttpClient _httpClient;
        const string url = "api/SendEmail/";

        public RSendEmail(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> SendEmailAsync(SendEmailViewModel oSendEmail)
        {
            //var json = JsonSerializer.Serialize(oSendEmail);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);

            var response = await _httpClient.PostAsJsonAsync(url, oSendEmail,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }
    }
}
