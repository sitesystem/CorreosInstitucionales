using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.Constantes
{
    public static class TipoExt
    {
        public static string GetNombre(this TipoDocumento documento)
        {
            switch (documento)
            {
                case TipoDocumento.CURP: return "CURP";
                case TipoDocumento.COM_INSCRIPCION: return "COMPROBANTE_INSCRIPCION";
                case TipoDocumento.CAP_ANTIVIRUS: return "CAPTURA_ANTIVIRUS";
                case TipoDocumento.CAP_BLOQUEO: return "CAPTURA_BLOQUEO";
                case TipoDocumento.CAP_ERROR: return "CAPTURA_ERROR";
            }

            return "ARCHIVO";
        }

        public static string GetNombre(this TipoPersonal personal)
        {
            switch (personal)
            {
                case TipoPersonal.ALUMNO:           return "ALUMNO";
                case TipoPersonal.EGRESADO:         return "EGRESADO";
                case TipoPersonal.MAESTRIA:         return "EGRESADO";
                case TipoPersonal.DOCENTE:          return "DOCENTE";
                case TipoPersonal.HONORARIOS:       return "HONORARIOS";
            }

            return "ADMINISTRATIVO";
        }

        public static TipoDocumento[] GetDocumentos(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.DESBLOQUEO_CUENTA: return [TipoDocumento.CAP_BLOQUEO, TipoDocumento.CAP_ANTIVIRUS];
                case TipoSolicitud.OTRO: return [TipoDocumento.CAP_ERROR];
            }

            return [];
        }

        public static TipoDatoActualizar[] GetDatosActualizar(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.DESBLOQUEO_CUENTA: return [TipoDatoActualizar.NINGUNO];
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return [TipoDatoActualizar.CORREO_PERSONAL];
                case TipoSolicitud.CAMBIO_CELULAR: return [TipoDatoActualizar.CELULAR];
                case TipoSolicitud.CORREO_EGRESADO: return [TipoDatoActualizar.CORREO_INSTITUCIONAL, TipoDatoActualizar.CONTRA];
                case TipoSolicitud.CREACION_ACTIVACION_CORREO_INST: return [TipoDatoActualizar.CORREO_INSTITUCIONAL, TipoDatoActualizar.CONTRA];
                case TipoSolicitud.RECUPERACION_CONTRA: return [TipoDatoActualizar.CONTRA];
            }

            return [TipoDatoActualizar.TODO];
        }
    }
}
