using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class WhatsAppController : Controller
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };
        const string url = "https://www.developers.upiicsa.ipn.mx:8081/api/SendWhatsApp";

        [HttpPost("send")]
        public async Task<IActionResult> WA_Send(RequestDTO_SendWhatsApp message)
        {
            Response<string> oResponse = new();

            HttpResponseMessage response = await WebUtils.Client.PostAsJsonAsync(url, message, options: _options);

            if(response.IsSuccessStatusCode)
            {
                oResponse.Success = 1;
                oResponse.Data = await response.Content.ReadAsStringAsync();
            }

            return Ok(oResponse);
        }
    }
}
