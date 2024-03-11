using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp
{
    public class RSendWhatsAppService(HttpClient httpClient) : ISendWhatsAppService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "https://www.developers.upiicsa.ipn.mx:8081/api/SendWhatsApp";

        public async Task<HttpResponseMessage> SendWhatsAppAsync(RequestDTO_SendWhatsApp oSendWhatsApp)
        {
            // Crear un HttpClientHandler personalizado
            //HttpClientHandler handler = new()
            //{
            //    // Desactivar la verificación de certificados
            //    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            //};

            //var json = JsonSerializer.Serialize(oSendWhatsApp);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oSendWhatsApp, options: _options);
            return response;
        }
    }
}
