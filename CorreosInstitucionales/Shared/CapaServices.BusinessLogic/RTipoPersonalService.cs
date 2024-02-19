using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RTipoPersonalService(HttpClient httpClient) : IGenericService<RequestViewModel_TipoPersonal>
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/TiposPersonal";

        public async Task<Response<List<RequestViewModel_TipoPersonal>>?> GetAllDataByStatusAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync($"{url}/filterByStatus/{filterByStatus}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestViewModel_TipoPersonal>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestViewModel_TipoPersonal>?> GetDataByIdAsync(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestViewModel_TipoPersonal>>($"{url}/filterById/{id}", options: _options);
            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(RequestViewModel_TipoPersonal oTipoPersonal)
        {
            //var json = JsonSerializer.Serialize(oTipoPersonal);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oTipoPersonal, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(RequestViewModel_TipoPersonal oTipoPersonal)
        {
            //var json = JsonSerializer.Serialize(oTipoPersonal);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync(url, oTipoPersonal, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate)
        {
            var response = await _httpClient.PutAsJsonAsync($"{url}/editByIdStatus/{id}/{isActivate}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }
    }
}
