using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaEntities.Request
{
    public class RequestDTO_CancelarSolicitud
    {
        public int IdSolicitud { get; set; }
        public string? MotivoCancelacion { get; set; }
    }
}
