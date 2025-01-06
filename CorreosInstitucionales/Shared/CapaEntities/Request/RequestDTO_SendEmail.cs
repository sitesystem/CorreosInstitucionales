using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_SendEmail
    {
        public string EmailTo { get; set; } = string.Empty;     // Correo Electrónico
        public string? Subject { get; set; } = string.Empty;    // Asunto
        public string? Body { get; set; } = string.Empty;       // Cuerpo o Contenido

        public override string ToString()
        {
            string trimmed = Body!.Length > 32 ? string.Join(" ", Body!.Substring(0, 32).Split(' ').Skip(1)) + "..." : Body;
            return $"{EmailTo}\t{Subject}\t{trimmed}";
        }
    }
}
