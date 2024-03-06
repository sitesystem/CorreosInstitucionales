using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class TipoEstadoSolicitud
    {
        public const int ALTA               = 1;
        public const int PENDIENTE          = 2;
        public const int EN_PROCESO         = 3;
        public const int ATENDIDA           = 4;
        public const int ENCUESTA_CALIDAD   = 5;
        public const int CANCELADA          = 6;
    }
}
