using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public enum TipoSolicitud
    {
        DESBLOQUEO_CUENTA = 1,
        CAMBIO_CORREO_PERSONAL = 2,
        CAMBIO_CELULAR = 3,
        CORREO_EGRESADO = 4,
        CREACION_ACTIVACION_CORREO_INST = 5,
        RECUPERACION_CONTRA = 6,
        OTRO = 7,
    }
}
