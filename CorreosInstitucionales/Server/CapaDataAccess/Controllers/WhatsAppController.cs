using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController(RSendWhatsAppService servicioWA): Controller
    {
        RSendWhatsAppService _wa = servicioWA;

        [HttpPost("send")]
        public async Task<IActionResult> WA_Send(RequestDTO_SendWhatsApp message)
        {
            Response<string> oResponse = await _wa.SendWhatsAppAsync(message);
            return Ok(oResponse);
        }
    }
}
