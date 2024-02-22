using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic
{
    public class REstadisticasService(HttpClient httpClient)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "/api/estadisticas";


        public async Task<Response<List<IntDataItem>>?> GetEstadosSolicitud(DateTime inicio, DateTime fin)
        {
            var response = await _httpClient.GetAsync(
                $"{url}/progreso/{inicio.Year}_{inicio.Month}_{inicio.Day}-{fin.Year}_{fin.Month}_{fin.Day}"
                );

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Response<List<IntDataItem>>>(content, options: _options);
            return result;
        }

    }
}
