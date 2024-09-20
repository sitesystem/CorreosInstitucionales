using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolDebug
{
    public class RDebugService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/debug";

        public async Task<Response<string>?> Auth()
        {
            //_httpClient.SendAsync()
            HttpResponseMessage response = await _httpClient.GetAsync($"{url}/auth" );

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            }

            return  new Response<string>() { Data = response.StatusCode.ToString() };
        }
    }
}
