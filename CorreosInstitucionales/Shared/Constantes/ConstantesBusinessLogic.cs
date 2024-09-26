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
                case TipoPersonal.MAESTRIA:         return "MAESTRÍA";
                case TipoPersonal.DOCENTE:          return "DOCENTE";
                case TipoPersonal.HONORARIOS:       return "HONORARIOS";
            }

            return "ADMINISTRATIVO";
        }

        public static string GetNombre(this TipoDatoXLSX dato)
        {
            switch(dato)
            {
                case TipoDatoXLSX.TODO: return "TODO";
                case TipoDatoXLSX.NINGUNO: return "NINGUNO";

                case TipoDatoXLSX.CORREO_PERSONAL: return "CORREO PERSONAL";
                case TipoDatoXLSX.CORREO_INSTITUCIONAL: return "CORREO INSTITUCIONAL";
                case TipoDatoXLSX.CONTRA: return "CONTRASEÑA";
                case TipoDatoXLSX.CELULAR: return "NÚMERO DE CELULAR ANTERIOR";
                case TipoDatoXLSX.EXTENSION: return "EXTENSIÓN";
                case TipoDatoXLSX.AREA: return "ÁREA";
                case TipoDatoXLSX.ID_EXTERNO: return "NUMERO DE EMPLEADO O NUMERO DE BOLETA SEGÚN SEA EL CASO";
                case TipoDatoXLSX.ACCION: return "ACCIÓN / RESPUESTA AC";
                case TipoDatoXLSX.CURP: return "CURP";

                case TipoDatoXLSX.CORREO_PERSONAL_NUEVO: return "CORREO PERSONAL NUEVO";
                case TipoDatoXLSX.CELULAR_NUEVO: return "NÚMERO DE CELULAR NUEVO";
                case TipoDatoXLSX.EXTENSION_NUEVO: return "NÚMERO DE EXTENSIÓN NUEVO ";
            }

            return "NO DEFINIDO?";
        }

        public static string GetPlantilla(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.CAMBIO_CELULAR:          return "sol_cambio_celular.xlsx";
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL:  return "sol_cambio_correo_personal.xlsx";
            }

            return "sol_alta_desbloqueo.xlsx";
        }

        public static string GetNombreExportacion(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.CAMBIO_CELULAR: return "sol_cambios_celular.xlsx";
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return "sol_cambios_correo_personal.xlsx";
            }

            return "sol_altas_y_desbloqueos.xlsx";
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

        public static TipoDatoXLSX[] GetDatosActualizar(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.DESBLOQUEO_CUENTA: return [TipoDatoXLSX.CONTRA];
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return [TipoDatoXLSX.CORREO_PERSONAL];
                case TipoSolicitud.CAMBIO_CELULAR: return [TipoDatoXLSX.CELULAR];
                case TipoSolicitud.CORREO_EGRESADO: return [TipoDatoXLSX.CORREO_INSTITUCIONAL, TipoDatoXLSX.CONTRA];
                case TipoSolicitud.CREACION_ACTIVACION_CORREO_INST: return [TipoDatoXLSX.CORREO_INSTITUCIONAL, TipoDatoXLSX.CONTRA];
                case TipoSolicitud.RECUPERACION_CONTRA: return [TipoDatoXLSX.CONTRA];
            }

            return [TipoDatoXLSX.TODO];
        }

        public static TipoDatoXLSX[] GetDatosExportar(this TipoSolicitud solicitud)
        {
            switch (solicitud)
            {
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return [TipoDatoXLSX.CURP, TipoDatoXLSX.CORREO_INSTITUCIONAL, TipoDatoXLSX.CORREO_PERSONAL, TipoDatoXLSX.CORREO_PERSONAL_NUEVO];
                case TipoSolicitud.CAMBIO_CELULAR: return [TipoDatoXLSX.CURP, TipoDatoXLSX.CORREO_INSTITUCIONAL, TipoDatoXLSX.CELULAR, TipoDatoXLSX.CELULAR_NUEVO];
            }

            return [TipoDatoXLSX.CURP,TipoDatoXLSX.ID_EXTERNO, TipoDatoXLSX.EXTENSION, TipoDatoXLSX.CORREO_PERSONAL];
        }
    }
}
