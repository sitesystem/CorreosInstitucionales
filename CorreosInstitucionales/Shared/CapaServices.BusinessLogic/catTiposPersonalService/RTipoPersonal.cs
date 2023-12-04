using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposPersonalService
{
    public class RTipoPersonal : ITipoPersonal
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Edificios";
        const string url = "/api/TiposPersonal/";

        public RTipoPersonal(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<TipoPersonalViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<TipoPersonalViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<TipoPersonalViewModel>?> GetDataByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<TipoPersonalViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(TipoPersonalViewModel oTipoPersonal)
        {
            var json = JsonSerializer.Serialize(oTipoPersonal);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            //var response = await _httpClient.PostAsJsonAsync<PisoViewModel>(url, oEdificio,
            //    new JsonSerializerOptions()
            //    {
            //        PropertyNameCaseInsensitive = true
            //    });

            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(TipoPersonalViewModel oTipoPersonal)
        {
            //var json = JsonSerializer.Serialize(oPiso);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oTipoPersonal,
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
