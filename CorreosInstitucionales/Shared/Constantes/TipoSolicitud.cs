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

    public static class TipoSolicitudMethods
    {
        public static TipoDocumento[] GetDocumentos(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.DESBLOQUEO_CUENTA:   return [TipoDocumento.CAP_BLOQUEO, TipoDocumento.CAP_ANTIVIRUS];
                case TipoSolicitud.OTRO:                return [TipoDocumento.CAP_ERROR];
            }

            return [];
        }
        
        public static TipoDatoActualizar[] GetDatosActualizar(this TipoSolicitud solicitud)
        {
            switch(solicitud)
            {
                case TipoSolicitud.DESBLOQUEO_CUENTA:               return [TipoDatoActualizar.NINGUNO];
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL:          return [TipoDatoActualizar.CORREO_PERSONAL];
                case TipoSolicitud.CAMBIO_CELULAR:                  return [TipoDatoActualizar.CELULAR];
                case TipoSolicitud.CORREO_EGRESADO:                 return [TipoDatoActualizar.CORREO_INSTITUCIONAL, TipoDatoActualizar.CONTRA]; 
                case TipoSolicitud.CREACION_ACTIVACION_CORREO_INST: return [TipoDatoActualizar.CORREO_INSTITUCIONAL, TipoDatoActualizar.CONTRA];
                case TipoSolicitud.RECUPERACION_CONTRA:             return [TipoDatoActualizar.CONTRA];
            }

            return [TipoDatoActualizar.TODO];
        }
    }
}
