using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class REscuelaService(HttpClient httpClient) : IGenericService<RequestViewModel_Escuela>
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        const string url = "/api/Escuelas";

        public async Task<Response<List<RequestViewModel_Escuela>>?> GetAllDataByStatusAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync($"{url}/filterByStatus/{filterByStatus}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestViewModel_Escuela>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestViewModel_Escuela>?> GetDataByIdAsync(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestViewModel_Escuela>>($"{url}/filterById/{id}", options: _options);
            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Escuela oEscuela)
        {
            //var json = JsonSerializer.Serialize(oEscuela);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oEscuela, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Escuela oEscuela)
        {
            //var json = JsonSerializer.Serialize(oEscuela);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync(url, oEscuela, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate)
        {
            var response = await _httpClient.PutAsJsonAsync($"{url}/editByIdStatus/{id}/{isActivate}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                });

            return response;
        }
    }
}
