using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class TipoSolicitud
    {
        public const int DESBLOQUEO_CUENTA                  = 1;
        public const int CAMBIO_CORREO_PERSONAL             = 2;
        public const int CAMBIO_CELULAR                     = 3;
        public const int CORREO_EGRESADO                    = 4;
        public const int CREACION_ACTIVACION_CORREO_INST    = 5;
        public const int RECUPERACION_CONTRA                = 6;
        public const int OTRO = 7;

        public static readonly Dictionary<int, int[]> Documentos = new()
        {
            {DESBLOQUEO_CUENTA,               [TipoDocumento.CAP_BLOQUEO, TipoDocumento.CAP_ANTIVIRUS] },
            {CAMBIO_CORREO_PERSONAL,          [] },
            {CAMBIO_CELULAR,                  [] },
            {CORREO_EGRESADO,                 [] },
            {CREACION_ACTIVACION_CORREO_INST, [] },
            {RECUPERACION_CONTRA,             [] },
            {OTRO,                            [TipoDocumento.CAP_ERROR] },
        };
    }
}
