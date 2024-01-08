using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail
{
    public class RSendEmailService(HttpClient httpClient) : ISendEmailService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "api/SendEmail/";

        public async Task<HttpResponseMessage> SendEmailAsync(RequestDTO_SendEmail oSendEmail)
        {
            //var json = JsonSerializer.Serialize(oSendEmail);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oSendEmail, options: _options);
            return response;
        }
    }
}
