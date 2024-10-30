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
        public static TipoPersonal[] ListaTipoAlumnoEgresado =
        [
            TipoPersonal.ALUMNO,
            TipoPersonal.EGRESADO,
            TipoPersonal.POSGRADO
        ];

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
                case TipoPersonal.POSGRADO:         return "POSGRADO";
                case TipoPersonal.DOCENTE:          return "DOCENTE";
                case TipoPersonal.HONORARIOS:       return "HONORARIOS";
            }

            return "ADMINISTRATIVO";
        }

        public static string GetNombre(this TipoDatoXLSX dato)
        {
            switch (dato)
            {
                case TipoDatoXLSX.TODO: return "TODO";
                case TipoDatoXLSX.NINGUNO: return "NINGUNO";

                case TipoDatoXLSX.CORREO_PERSONAL: return "CORREO PERSONAL ANTERIOR";
                case TipoDatoXLSX.CORREO_INSTITUCIONAL: return "CORREO INSTITUCIONAL";
                case TipoDatoXLSX.CONTRA: return "CONTRASEÑA";
                case TipoDatoXLSX.CELULAR: return "NÚMERO DE CELULAR ANTERIOR";
                case TipoDatoXLSX.EXTENSION: return "EXTENSIÓN";
                case TipoDatoXLSX.AREA: return "ÁREA";
                case TipoDatoXLSX.ID_EXTERNO: return "NUMERO DE EMPLEADO O NUMERO DE BOLETA SEGÚN SEA EL CASO";
                case TipoDatoXLSX.ACCION: return "ACCIÓN / RESPUESTA AC";
                case TipoDatoXLSX.CURP: return "CURP";

                case TipoDatoXLSX.CORREO_PERSONAL_NUEVO: return "CORREO PERSONAL ACTUAL";
                case TipoDatoXLSX.CELULAR_NUEVO: return "NÚMERO DE CELULAR ACTUAL";
                case TipoDatoXLSX.EXTENSION_NUEVO: return "NÚMERO DE EXTENSIÓN NUEVO ";
            }

            return "NO DEFINIDO?";
        }

        public static bool EsAlumnoOEgresado(this TipoPersonal tipoPersonal)
        {
            return ListaTipoAlumnoEgresado.Contains(tipoPersonal);
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
                case TipoSolicitud.CAMBIO_CELULAR: return "SOLICITUD_DE_CAMBIO_DE_CELULAR ";
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return "SOLICITUD_DE_CAMBIO_CORREO_PERSONAL ";
                case TipoSolicitud.DESBLOQUEO_CUENTA: return "SOLICITUD_DE_DESBLOQUEO_DE_CUENTA ";
            }

            return "SOLICITUD_DE_CUENTAS_INSTITUCIONALES ";
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
                case TipoSolicitud.CAMBIO_CORREO_PERSONAL: return [];
                case TipoSolicitud.CAMBIO_CELULAR: return [];
            }

            return [TipoDatoXLSX.TODO];
        }
        public static List<TipoDatoXLSX> GetDatosExportar(this TipoSolicitud solicitud)
        {
            List<TipoDatoXLSX> datos = 
            [
                TipoDatoXLSX.CURP,
                TipoDatoXLSX.ID_EXTERNO,
            ];

            switch (solicitud)
            {
                case TipoSolicitud.CREACION_ACTIVACION_CORREO_INST:
                case TipoSolicitud.RECUPERACION_CONTRA:
                case TipoSolicitud.CORREO_EGRESADO:
                    datos.AddRange
                    (
                        [
                            TipoDatoXLSX.EXTENSION_NUEVO, 
                            TipoDatoXLSX.CORREO_PERSONAL_NUEVO
                        ]
                    );
                    break;

                case TipoSolicitud.DESBLOQUEO_CUENTA:
                    datos.AddRange
                    (
                        [
                            TipoDatoXLSX.EXTENSION_NUEVO,
                            TipoDatoXLSX.CORREO_PERSONAL_NUEVO,
                            TipoDatoXLSX.CORREO_INSTITUCIONAL
                        ]
                    );
                    break;

                case TipoSolicitud.OTRO:
                    datos.AddRange
                    (
                        [
                            TipoDatoXLSX.EXTENSION_NUEVO,
                            TipoDatoXLSX.CORREO_PERSONAL_NUEVO,
                            TipoDatoXLSX.CORREO_INSTITUCIONAL,
                            TipoDatoXLSX.CELULAR_NUEVO
                        ]
                    );
                    break;

                case TipoSolicitud.CAMBIO_CELULAR:
                    datos.AddRange
                    (
                        [
                            TipoDatoXLSX.CORREO_INSTITUCIONAL,
                            TipoDatoXLSX.CELULAR,
                            TipoDatoXLSX.CELULAR_NUEVO
                        ]
                    );
                    break;

                case TipoSolicitud.CAMBIO_CORREO_PERSONAL:
                    datos.AddRange
                    (
                        [
                            TipoDatoXLSX.CORREO_INSTITUCIONAL,
                            TipoDatoXLSX.CORREO_PERSONAL,
                            TipoDatoXLSX.CORREO_PERSONAL_NUEVO
                        ]
                    );
                    break;
            }

            return datos;
        }
    }
}
