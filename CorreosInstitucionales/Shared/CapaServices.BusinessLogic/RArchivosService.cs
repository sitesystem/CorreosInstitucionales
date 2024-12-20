﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.Constantes;

using static System.Collections.Specialized.BitVector32;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class RArchivosService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        const string url = "/api/archivos";

        public async Task<Response<string>?> SubirAnuncio(ContentData data)
        {
            HttpResponseMessage response;

            using (MultipartFormDataContent fdContent = [])
            {
                using (StreamContent fileContent = new StreamContent(new MemoryStream(data.Bytes)))
                {
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(data.ContentType);
                    fdContent.Add(fileContent, name: "\"file\"", data.FileName);
                    response = await _httpClient.PostAsync($"{url}/subir/anuncio", fdContent);
                }
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<string?> SubirDocumento(TipoDocumento documento, IBrowserFile file)
        {
            string? result = null;
            using (MultipartFormDataContent content = [])
            {
                using (StreamContent fileContent = new(file.OpenReadStream()))
                {
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                    content.Add(fileContent, name: "\"file\"", file.Name);

                    //SUBIR 
                    HttpResponseMessage httpResponse = await _httpClient.PostAsync($"{url}/subir/{(int)documento}", content);

                    string json = await httpResponse.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<Response<string?>>(json, options: _options);

                    if (response is not null)
                    {
                        result = response.Data;
                    }
                }
            }

            return result;
        }

        public async Task<Response<string>?> PostFile(string extension, string action, MultipartFormDataContent formData)
        {
            var response = await _httpClient.PostAsync($"{url}/{extension}/{action}", formData);

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<Response<string>?> NewFile(string extension, string action)
        {
            var response = await _httpClient.GetAsync($"{url}/{extension}/{action}");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<string>>(content, options: _options);
            return result;
        }

        public async Task<Response<List<WebUtils.Link>>?> NewFileFromSelection<T>(string extension, string action, List<T> selected)
        {
            Response<List<WebUtils.Link>>? result = new();

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{url}/{extension}/{action}", selected);

                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Response<List<WebUtils.Link>>>(content, options: _options);
            }
            catch(Exception ex)
            {
                result!.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            
            return result;
        }

        public async Task<Response<List<WebUtils.Link>>?> NewFileExportFromSelection<T>(string extension, string action, TExport<T> selected)
        {
            Response<List<WebUtils.Link>>? result = new();

            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{url}/{extension}/{action}", selected);

                var content = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<Response<List<WebUtils.Link>>>(content, options: _options);
            }
            catch (Exception ex)
            {
                result!.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return result;
        }

        public async Task<Response<List<string>>?> ArreglarEnlacesRotos()
        {
            var response = await _httpClient.GetAsync($"{url}/*/arreglar_rotos");
            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Response<List<string>>>(content, options: _options);
        }
    }
}
