using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RArchivosService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/archivos";

        
        public async Task<Response<string>?> NewFile(string extension, string action)
        {
            var response = await _httpClient.GetAsync($"{url}/{extension}/{action}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<Response<List<string>>?> NewFileFromSelection<T>(string extension, string action, List<T> selected)
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/{extension}/{action}", selected);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<string>>>(content, options: _options);
            return result;
        }

    }
}
