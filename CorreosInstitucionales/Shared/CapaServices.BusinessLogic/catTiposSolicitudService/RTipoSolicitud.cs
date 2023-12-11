using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.catTiposSolicitudService
{
    public class RTipoSolicitud : ITipoSolicitud
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Edificios";
        const string url = "/api/TiposSolicitud/";

        public RTipoSolicitud(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<TipoSolicitudViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<TipoSolicitudViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<Response<TipoSolicitudViewModel>?> GetDataByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<TipoSolicitudViewModel>>(url + "filterById/" + id,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(TipoSolicitudViewModel oTipoSolicitud)
        {
            var json = JsonSerializer.Serialize(oTipoSolicitud);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            //var response = await _httpClient.PostAsJsonAsync<TipoSolicitudViewModel>(url, oTipoSolicitud,
            //    new JsonSerializerOptions()
            //    {
            //        PropertyNameCaseInsensitive = true
            //    });

            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(TipoSolicitudViewModel oTipoSolicitud)
        {
            //var json = JsonSerializer.Serialize(oTipoSolicitud);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oTipoSolicitud,
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
