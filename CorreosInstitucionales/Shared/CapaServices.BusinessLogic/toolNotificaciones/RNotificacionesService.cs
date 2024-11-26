using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolNotificaciones
{
    public class RNotificacionesService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        const string url = "/api/notificaciones";

        public async Task<Response<string>?> Notificar(Notificacion notificacion)
        {
            Response<string>? result = null;

            var response = await _httpClient.PostAsJsonAsync($"{url}/enviar", notificacion);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            }
            
            return result;
        }

        public async Task<Response<string>?> EnviarCorreo(RequestDTO_SendEmail correo)
        {
            Response<string>? result = null;

            var response = await _httpClient.PostAsJsonAsync($"{url}/enviarCorreo", correo);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            }

            return result;
        }


        public async Task<Response<string>?> EnviarWA(RequestDTO_SendWhatsApp wa)
        {
            Response<string>? result = null;

            var response = await _httpClient.PostAsJsonAsync($"{url}/enviarWA", wa);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            }

            return result;
        }
    }
}
