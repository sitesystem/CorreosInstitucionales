using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarrerasService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catCarrerasService
{
    public class RCarrera : ICarrera
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Edificios";
        const string url = "/api/Carreras/";
        public RCarrera(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Response<List<CarreraViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<CarreraViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<CarreraViewModel>?> GetDataByAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<CarreraViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }
        public async Task<HttpResponseMessage> AddDataAsync(CarreraViewModel oCarrera)
        {
            var json = JsonSerializer.Serialize(oCarrera);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(CarreraViewModel oCarrera)
        {
            var response = await _httpClient.PutAsJsonAsync(url, oCarrera,
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
