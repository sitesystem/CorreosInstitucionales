using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RArchivosService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/archivos";

        public async Task<Response<string>?> PostFile(string extension, string action, MultipartFormDataContent formData)
        {
            var response = await _httpClient.PostAsync($"{url}/{extension}/{action}", formData);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<Response<string>?> NewFile(string extension, string action)
        {
            var response = await _httpClient.GetAsync($"{url}/{extension}/{action}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<Response<List<WebUtils.Link>>?> NewFileFromSelection<T>(string extension, string action, List<T> selected)
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/{extension}/{action}", selected);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<WebUtils.Link>>>(content, options: _options);
            return result;
        }

        public async Task<Response<List<string>>?> ArreglarEnlacesRotos()
        {
            var response = await _httpClient.GetAsync($"{url}/*/arreglar_rotos");
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response<List<string>>>(content, options: _options);
        }


    }
}
