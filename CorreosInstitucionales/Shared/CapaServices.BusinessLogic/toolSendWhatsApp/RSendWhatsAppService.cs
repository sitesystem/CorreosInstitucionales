using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.Utils;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp
{
    public class RSendWhatsAppService(HttpClient httpClient) : ISendWhatsAppService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/WhatsApp";

        public async Task<HttpResponseMessage> SendWhatsAppAsync(RequestDTO_SendWhatsApp oSendWhatsApp)
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/send", oSendWhatsApp, options: _options);
            return response;
        }
    }
}
