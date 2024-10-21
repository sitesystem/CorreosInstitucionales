using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RUsuarioService(HttpClient httpClient) : IGenericService<RequestDTO_Usuario>
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        const string url = "/api/Usuarios";

        public async Task<Response<List<RequestDTO_Usuario>>?> GetAllDataByStatusAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync($"{url}/filterByStatus/{filterByStatus}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestDTO_Usuario>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Usuario>?> GetDataByIdAsync(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Usuario>>($"{url}/filterById/{id}", options: _options);
            return result;
        }

        public async Task<Response<object>?> GetCountDataAsync()
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/countData", _options);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<object>?>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Usuario>?> ValidateByEmailCURP(string correo, string curp)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Usuario>>($"{url}/filterByEmailCURP/{correo}/{curp}", options: _options);
            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(RequestDTO_Usuario oUsuario)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oUsuario, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(RequestDTO_Usuario oUsuario)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync(url, oUsuario, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> ResetPassword(string correoPersonal, string curp)
        {
            // var json = JsonSerializer.Serialize(correoPersonal);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync($"{url}/resetPassword/{correoPersonal}/{curp}",
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true,
                     Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                     WriteIndented = true
                 });

            return response;
        }

        public async Task<HttpResponseMessage> ChangePassword(int id, string newPassword)
        {
            // var json = JsonSerializer.Serialize(correoPersonal);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync($"{url}/changePassword/{id}/{newPassword}",
                 new JsonSerializerOptions()
                 {
                     PropertyNameCaseInsensitive = true,
                     Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                     WriteIndented = true
                 });

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

        public async Task<Response<string>?> GenerarRandom(int cantidad)
        {
            var response = await _httpClient.GetAsync($"{url}/generar_random/{cantidad}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }
    }
}
