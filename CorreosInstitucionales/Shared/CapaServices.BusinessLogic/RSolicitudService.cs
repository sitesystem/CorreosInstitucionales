﻿using System;
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
    public class RSolicitudService(HttpClient httpClient) : IGenericService<RequestDTO_Solicitud>
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        const string url = "/api/Solicitudes";

        public async Task<Response<List<RequestDTO_Solicitud>>?> GetAllDataByStatusAsync(bool filterByStatus)
        {
            var response = await _httpClient.GetAsync($"{url}/filterByStatus/{filterByStatus}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestDTO_Solicitud>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Solicitud>?> GetDataByIdAsync(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Solicitud>>($"{url}/filterById/{id}", options: _options);
            return result;
        }

        public async Task<Response<List<RequestDTO_Solicitud>>?> GetAllDataByIdUsuarioAsync(int filterByIdUsuario)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<List<RequestDTO_Solicitud>>>($"{url}/filterByIdUsuario/{filterByIdUsuario}", options: _options);
            return result;
        }

        public async Task<Response<List<RequestDTO_Solicitud>>?> GetAllDataByProgressAsync(int[] progress)
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/filterByProgress", progress, _options);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<RequestDTO_Solicitud>>>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Solicitud>?> GetDataByIdUsuarioStatusAsync(int filterByIdUsuarioStatus)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Solicitud>>($"{url}/filterByIdUsuarioStatus/{filterByIdUsuarioStatus}", options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Solicitud>?> GetDataByIdUsuarioLastTicket(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Solicitud>>($"{url}/filterByIdUsuarioLastTicket/{id}", options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Solicitud>?> GetDataByIdUsuarioLastTicketRespuesta(int? id)
        {
            var result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Solicitud>>($"{url}/filterByIdUsuarioLastTicketRespuesta/{id}", options: _options);
            return result;
        }

        public async Task<Response<object>?> GetCountDataByProgressAsync(int idUser, int[] progress)
        {
            var response = await _httpClient.PostAsJsonAsync($"{url}/countDataByIdUserProgress/{idUser}", progress, _options);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<object>?>(content, options: _options);
            return result;
        }

        public async Task<Response<RequestDTO_Solicitud?>?> GetDataByToken(string token)
        {
            Response<RequestDTO_Solicitud?>? result = new() { Success = 0 };

            try
            {
                result = await _httpClient.GetFromJsonAsync<Response<RequestDTO_Solicitud?>>($"{url}/filterByToken/{token}");
                
            }catch(Exception ex)
            {
                result!.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            
            return result;
        }

        public async Task<HttpResponseMessage> AddDataAsync(RequestDTO_Solicitud oSolicitud)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PostAsync(url, content);
            var response = await _httpClient.PostAsJsonAsync(url, oSolicitud, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditDataAsync(RequestDTO_Solicitud oSolicitud)
        {
            // var json = JsonSerializer.Serialize(oUsuario);
            // var content = new StringContent(json, Encoding.UTF8, "application/json");
            // var response = await _httpClient.PutAsync(url, content);
            var response = await _httpClient.PutAsJsonAsync(url, oSolicitud, options: _options);
            return response;
        }

        public async Task<HttpResponseMessage> EditStatusByIdAsync(int id, int status)
        {
            var response = await _httpClient.PutAsJsonAsync($"{url}/editStatusById/{id}/{status}",
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                });

            return response;
        }

        public async Task<HttpResponseMessage> EncuestaCalidadAsync(RequestDTO_EncuestaCalidad oSolicitudEncuestaCalidad)
        {
            var response = await _httpClient.PutAsJsonAsync($"{url}/encuesta/calidad", oSolicitudEncuestaCalidad, options: _options);
            return response;
        }

        public Task<HttpResponseMessage> EnableDisableDataByIdAsync(int id, bool isActivate)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpResponseMessage> Cancelar(RequestDTO_FinalizarSolicitud oCancelarSolicitud) // KeyValuePair<int,string> datos)
        {
            return await _httpClient.PatchAsJsonAsync($"{url}/finalizar", oCancelarSolicitud, options: _options);
        }

        public async Task<HttpResponseMessage> Finalizar(RequestDTO_FinalizarSolicitud oFinalizarSolicitud) // KeyValuePair<int,string> datos)
        {
            return await _httpClient.PatchAsJsonAsync($"{url}/finalizar", oFinalizarSolicitud, options: _options);
        }
    }
}
