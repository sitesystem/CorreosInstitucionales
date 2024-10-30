using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RPlantillaService(HttpClient httpClient) : IGenericService<RequestViewModel_Plantilla>
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/plantillas";

        public async Task<HttpResponseMessage> AddDataAsync(RequestViewModel_Plantilla oObject)
        {
            var response = await _httpClient.PostAsJsonAsync(url, oObject, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(RequestViewModel_Plantilla oObject)
        {
            var response = await _httpClient.PutAsJsonAsync(url, oObject, options: _options);
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

        public async Task<Response<List<RequestViewModel_Plantilla>>?> GetAllDataByStatusAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync($"{url}/filterByStatus/{filterByStatus}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestViewModel_Plantilla>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestViewModel_Plantilla>?> GetDataByIdAsync(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestViewModel_Plantilla>>($"{url}/filterById/{id}", options: _options);
            return result;
        }

        public async Task<Response<List<RequestViewModel_Plantilla>>?> GetDataByFilter(int filter)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<List<RequestViewModel_Plantilla>>>($"{url}/filter/{filter}", options: _options);
            return result;
        }
    }
}
