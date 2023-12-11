using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request
{
    public class SendEmailViewModel
    {
        public string EmailTo { get; set; } = null!;  // Correo Electrónico
        public string? Subject { get; set; }        // Asunto
        public string? Body { get; set; }           // Cuerpo o Contenido
    }
}
