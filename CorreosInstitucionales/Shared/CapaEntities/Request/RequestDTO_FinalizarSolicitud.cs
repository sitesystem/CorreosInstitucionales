using CorreosInstitucionales.Shared.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_FinalizarSolicitud
    {
        public int IdSolicitud { get; set; }
        public string? Mensaje { get; set; }
        public int Estado { get; set; }
    }
}
