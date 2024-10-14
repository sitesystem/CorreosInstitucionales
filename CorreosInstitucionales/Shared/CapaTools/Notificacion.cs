using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendEmail;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.Utils;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class Notificacion
    {
        public RequestDTO_SendEmail correo = new()
        {
            Subject = "Asunto",
            EmailTo = "postmaster@localhost",
            Body = "NO DEFINIDO"
        };

        public RequestDTO_SendWhatsApp wa = new()
        {
            Message = "PRUEBA",
            Number = "5500000000"
        };

        public bool EsCorreoPrueba()
        {
            return Dominios.EsCorreoDePrueba(correo.EmailTo);
        }

        public bool EsWAPrueba()
        {
            return wa.Number == "5500000000";
        }

        public async Task<List<string>> Enviar(ISendEmailService servicioCorreo, WhatsApp WA)
        {
            string id = Guid.NewGuid().ToString();
            string archivo = $"{ServerFS.GetBaseDir(true)}/repositorio/envios/{id}.txt";

            List<string> log = [];

            if (EsCorreoPrueba())
            {
                log.Add($"[CORREO DE PRUEBA] {correo.EmailTo}");
            }
            else
            {
                _ = Task.Run(() => servicioCorreo.SendEmailAsync(correo));
                log.Add($"[CORREO ENVIADO:] {correo.EmailTo}");
            }

            if (EsWAPrueba())
            {
                log.Add($"[WA DE PRUEBA] {wa.Message}");
            }
            else
            {
                log.Add($"[ENVIO DE WA] {correo.EmailTo}");
                Response<string> response = await WA.SendMessage(wa);
                log.Add(response.Success == 1 ? "[OK]" : "[ERROR]");
            }
            
            System.IO.File.WriteAllText(archivo, string.Join(Environment.NewLine, log));

            return log;
        }
    }
}
