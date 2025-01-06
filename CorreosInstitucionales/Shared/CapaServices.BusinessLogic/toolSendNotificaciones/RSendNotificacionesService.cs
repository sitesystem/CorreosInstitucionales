using CorreosInstitucionales.Shared.CapaTools;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.Constantes;
using System.Numerics;
using CorreosInstitucionales.Shared.CapaDataAccess;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendNotificaciones
{
    public class RSendNotificacionesService(
        RSendEmailService servicioCorreo, 
        RSendWhatsAppService servicioWA)
    {
        RSendEmailService _servicioCorreo = servicioCorreo;
        RSendWhatsAppService _servicioWA = servicioWA;

        public async Task<Response<string>> EnviarCorreoAsync(
            RequestDTO_SendEmail email, 
            IDictionary<string, byte[]>? attachments = null)
        {
            return await _servicioCorreo.SendEmailAsync(email, attachments);
        }

        public async Task<Response<string>> EnviarWAAsync(RequestDTO_SendWhatsApp wa)
        {
            return await _servicioWA.SendWhatsAppAsync(wa);
        }

        public async Task<Response<string>> NotificarUsuario(Dictionary<string, object?> datos, MpTbUsuario usuario, int filtro, int estado = 1)
        {
            PlantillaManager plantillas = new PlantillaManager(AppCache.Plantillas);
            Response<Notificacion?> notificacion = plantillas.GetNotificacion(datos, estado, filtro);

            Response<string> response = new() { Success = 0 };

            if (notificacion != null && notificacion.Data != null && notificacion.Success == 1)
            {
                notificacion.Data.correo.EmailTo = usuario.UsuCorreoPersonalCuentaActual;
                notificacion.Data.wa.Number = usuario.UsuNoCelularActual;

                response = await EnviarAsync(notificacion.Data);
            }

            return response;
        }

        public async Task<Response<string>> EnviarAsync(Notificacion notificacion)
        {
            Response<string> notificacion_correo = await _servicioCorreo.SendEmailAsync(notificacion.correo);
            Response<string> notificacion_wa = await _servicioWA.SendWhatsAppAsync(notificacion.wa);

            int notificado = notificacion_correo.Success == 1 || notificacion_wa.Success == 1 ? 1 : 0;
            string mensajes = notificacion_correo.Data + Environment.NewLine + notificacion_wa.Data;

            return new Response<string>() { Success = notificado, Data = mensajes };
        }

        public async Task<Response<string>> EnviarAsync(IEnumerable<Notificacion> notificaciones)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/envios/{id}.txt";

            StringBuilder log = new();

            Response<string> response = new() { Success = 1 };
            Response<string> response_correo = new() { Success = 0 };
            Response<string> response_wa = new() { Success = 0 };

            foreach (Notificacion notificacion in notificaciones)
            {
                if (notificacion.EsCorreoPrueba())
                {
                    log.AppendLine($"[CORREO DE PRUEBA] {notificacion.correo.EmailTo}");
                }
                else
                {
                    //_ = Task.Run(() => _servicioCorreo.SendEmailAsync(notificacion.correo));
                    response_correo = await _servicioCorreo.SendEmailAsync(notificacion.correo);
                }

                if (notificacion.EsWAPrueba())
                {
                    log.AppendLine($"[WA DE PRUEBA] {notificacion.wa.Message}");
                }
                else
                {
                    response_wa = await _servicioWA.SendWhatsAppAsync(notificacion.wa);
                }

                log.AppendLine($"{(response_correo.Success == 1 ? "[OK]" : "[ERROR]")} ENVÍO DE CORREO A {notificacion.correo.EmailTo}");
                log.AppendLine($"{(response_wa.Success == 1 ? "[OK]" : "[ERROR]")} ENVÍO DE WA A {notificacion.wa.Number}");
            }//FOREACH solicitud

            response.Data = log.ToString();
            System.IO.File.WriteAllText(archivo, response.Data);

            return response;
        }//NOTIFICAR

        public async Task<string> EnvioMasivoAsync(
            IEnumerable<MtTbSolicitudesTicket> lista,
            int tipoEstado
        )
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/envios/{id}.txt";

            StringBuilder log = new();

            Response<Notificacion?> notificacion;
            Response<string> response;
                        
            int filtro = 0;

            Dictionary<string, object?> datos = new()
            {
                {"solicitud", null },
                {"usuario", null },
                {"escuela", AppCache.Escuela }
            };

            PlantillaManager plantillas = new PlantillaManager(AppCache.Plantillas);

            foreach (MtTbSolicitudesTicket solicitud in lista)
            {
                filtro = (TipoEstadoSolicitud)solicitud.SolIdEstadoSolicitud != TipoEstadoSolicitud.ATENDIDA ||
                        ((TipoPersonal)solicitud.SolIdUsuarioNavigation!.UsuIdTipoPersonal).EsAlumnoOEgresado() ? 0 : 1;

                datos["solicitud"] = solicitud;
                datos["usuario"] = solicitud.SolIdUsuarioNavigation;

                notificacion = plantillas.GetNotificacion(datos, tipoEstado, filtro);

                if(notificacion.Success == 1)
                {
                    notificacion.Data!.correo.EmailTo = solicitud.SolIdUsuarioNavigation!.UsuCorreoPersonalCuentaActual;
                    notificacion.Data!.wa.Number = solicitud.SolIdUsuarioNavigation.UsuNoCelularActual!.Replace(" ", string.Empty);

                    response = await EnviarAsync(notificacion.Data!);

                    if (response.Success == 1)
                    {
                        log.AppendLine($"SE NOTIFICÓ A {solicitud.SolIdUsuarioNavigation.UsuCorreoPersonalCuentaActual}");
                    }

                    if (!string.IsNullOrEmpty(response.Data))
                    {
                        log.AppendLine(response.Data);
                        log.AppendLine("================");
                    }
                }
                else
                {
                    log.AppendLine($"[ERROR] NO SE PUDO GENERAR LA NOTIFICACIÓN:\n{notificacion.Message}");
                }
            }//FOREACH solicitud

            string data = log.ToString();
            System.IO.File.WriteAllText(archivo, data);

            return data;
        }
    }
}
