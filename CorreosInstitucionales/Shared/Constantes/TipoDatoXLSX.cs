using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public enum TipoDatoXLSX
    {
        TODO                    = -1,
        NINGUNO                 = 0,

        CORREO_PERSONAL         = 1,
        CORREO_INSTITUCIONAL    = 2,
        CONTRA                  = 3,
        CELULAR                 = 4,
        EXTENSION               = 5,
        AREA                    = 6,
        ID_EXTERNO              = 7,
        ACCION                  = 8,
        CURP                    = 9,

        CORREO_PERSONAL_NUEVO       = 11,
        CELULAR_NUEVO               = 14,
        EXTENSION_NUEVO             = 15,
    }
}
