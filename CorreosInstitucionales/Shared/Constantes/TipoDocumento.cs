using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class TipoDocumento
    {
        public const int CURP               = 0;
        public const int COM_INSCRIPCION    = 1;
        public const int CAP_ANTIVIRUS      = 2;
        public const int CAP_BLOQUEO        = 3;
        public const int CAP_ERROR          = 4;

        public static readonly Dictionary<int, string> Nombre = new()
        {
            {CURP,"CURP" },
            {COM_INSCRIPCION,"COMPROBANTE_INSCRIPCION" },
            {CAP_ANTIVIRUS,"CAPTURA_ANTIVIRUS" },
            {CAP_BLOQUEO,"CAPTURA_BLOQUEO" },
            {CAP_ERROR,"CAPTURA_ERROR" },
        };
    }
}
