using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catAreasDeptosService
{
    public class RAreaDepto : IAreaDepto
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/AreasDeptos";
        const string url = "/api/AreasDeptos/";

        public RAreaDepto(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<AreaDeptoViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<AreaDeptoViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<AreaDeptoViewModel>?> GetDataByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<AreaDeptoViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(AreaDeptoViewModel oAreaDepto)
        {
            //var json = JsonSerializer.Serialize(oAreaDepto);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PostAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oAreaDepto,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(AreaDeptoViewModel oAreaDepto)
        {
            //var json = JsonSerializer.Serialize(oAreaDepto);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oAreaDepto,
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
