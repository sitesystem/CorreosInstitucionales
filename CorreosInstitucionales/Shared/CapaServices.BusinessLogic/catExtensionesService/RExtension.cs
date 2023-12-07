using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catExtensionService
{
    public class RExtension : IExtension
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Extensiones";
        const string url = "/api/Extensiones/";

        public RExtension(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<ExtensionViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<ExtensionViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<ExtensionViewModel>?> GetDataByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<ExtensionViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(ExtensionViewModel oExtension)
        {
            var json = JsonSerializer.Serialize(oExtension);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            //var response = await _httpClient.PostAsJsonAsync<ExtensionViewModel>(url, oExtension,
            //    new JsonSerializerOptions()
            //    {
            //        PropertyNameCaseInsensitive = true
            //    });

            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(ExtensionViewModel oExtension)
        {
            //var json = JsonSerializer.Serialize(oExtension);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oExtension,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return response;
        }

        public async Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate)
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
