using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.tbUsuariosService
{
    public class RUsuario : IUsuario
    {
        private readonly HttpClient _httpClient;
        //const string url = "https://localhost:7271/api/Usuarios";
        const string url = "/api/Usuarios/";

        public RUsuario(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<UsuarioViewModel>>?> GetAllDataAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync(url + "filterByStatus/" + filterByStatus);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<UsuarioViewModel>>>(content,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            return result;
        }

        public async Task<Response<UsuarioViewModel>?> GetDataByIdAsync(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<UsuarioViewModel>>(url + "filterById/" + id,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<Response<UsuarioViewModel>?> ValidateByEmailCURP(string correo, string curp)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<UsuarioViewModel>>(url + "filterByEmailCURP/" + correo + "/" + curp,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(UsuarioViewModel oUsuario)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PostAsync(url, content);

            var response = await _httpClient.PostAsJsonAsync(url, oUsuario,
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(UsuarioViewModel oUsuario)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url, oUsuario,
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true
                 });

            return response;
        }

        public async Task<HttpResponseMessage> ResetPassword(string correoPersonal)
        {
            // var json = JsonSerializer.Serialize(correoPersonal);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);

            var response = await _httpClient.PutAsJsonAsync(url + "resetPassword/" + correoPersonal,
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
