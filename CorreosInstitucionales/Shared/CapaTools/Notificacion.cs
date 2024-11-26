using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.Constantes;

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
    }
}
