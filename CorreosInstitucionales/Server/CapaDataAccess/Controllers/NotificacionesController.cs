using CorreosInstitucionales.Shared.CapaDataAccess;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendNotificaciones;
using CorreosInstitucionales.Shared.CapaTools;
using Microsoft.AspNetCore.Mvc;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NotificacionesController(RSendNotificacionesService servicioNotificacion) : ControllerBase
    {
        RSendNotificacionesService _servicioNotificacion = servicioNotificacion;

        [HttpPost("enviar")]
        public async Task<IActionResult> Notificar(Notificacion notificacion)
        {
            Response<string> oResponse = await _servicioNotificacion.EnviarAsync(notificacion);
            return Ok(oResponse);
        }

        [HttpPost("enviarCorreo")]
        public async Task<IActionResult> EnviarCorreo(
                RequestDTO_SendEmail correo
            )
        {
            Response<string> oResponse = await _servicioNotificacion.EnviarCorreoAsync(correo);
            return Ok(oResponse);
        }

        [HttpPost("enviarWA")]
        public async Task<IActionResult> EnviarWA(RequestDTO_SendWhatsApp wa)
        {
            Response<string> oResponse = await _servicioNotificacion.EnviarWAAsync(wa);
            return Ok(oResponse);
        }
    }
}
