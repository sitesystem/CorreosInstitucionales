using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catEscuelasService
{
    public class REscuela : IEscuela
    {
        private readonly HttpClient _httpClient;
        const string url = "/api/Escuelas/";

        public REscuela(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<EscuelaViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<EscuelaViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<EscuelaViewModel>?> GetDataByAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<EscuelaViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }
        public async Task<HttpResponseMessage> AddDataAsync(EscuelaViewModel oEscuela)
        {
            var json = JsonSerializer.Serialize(oEscuela);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(EscuelaViewModel oEscuela)
        {
            var response = await _httpClient.PutAsJsonAsync(url, oEscuela,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return response;
        }

        public async Task<HttpResponseMessage> EnableDisableDataById(int id, bool isActivate)
        {
            var response = await _httpClient.PutAsJsonAsync(url + "editByIdStatus/" + id + "/" + isActivate,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }
    }
}