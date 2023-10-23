using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catPisosService
{
    public class RPiso : IPiso
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Edificios";
        const string url = "/api/Pisos/";

        public RPiso(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Response<List<PisoViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<PisoViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<PisoViewModel>?> GetDataByAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<PisoViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(PisoViewModel oPiso)
        {
            var json = JsonSerializer.Serialize(oPiso);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(PisoViewModel oPiso)
        {
            var response = await _httpClient.PutAsJsonAsync(url, oPiso,
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
