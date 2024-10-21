using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController(HttpClient _client): Controller
    {
        const string url = "https://www.developers.upiicsa.ipn.mx:8081/api/SendWhatsApp";

        protected readonly HttpClient client = _client;
        protected readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        
        [HttpPost("send")]
        public async Task<IActionResult> WA_Send(RequestDTO_SendWhatsApp message)
        {
            Response<string> oResponse = new();

            if(message.Number == "5500000000")
            {
                oResponse.Success = 1;
                oResponse.Data = "EL MENSAJE NO SE ENVIÓ DADO QUE ES UN NÚMERO DE PRUEBA.";
                return Ok(oResponse);
            }

            HttpResponseMessage response = await client.PostAsJsonAsync(url, message, options: _options);

            if(response.IsSuccessStatusCode)
                oResponse.Success = 1;

            oResponse.Data = await response.Content.ReadAsStringAsync();

            return Ok(oResponse);
        }
    }
}
