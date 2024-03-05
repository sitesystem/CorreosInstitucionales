using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorreosInstitucionales.Shared.Constantes;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class SolicitudConfig
    {
        public static Dictionary<int, int[]> Documentos = new Dictionary<int, int[]>()
        {
            {
                TipoSolicitud.DESBLOQUEO_CUENTA, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION, 
                    TipoDocumento.C_ANTIVIRUS 
                ] 
            },

            {
                TipoSolicitud.CAMBIO_CORREO_PERSONAL, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION
                ]
            },

            {
                TipoSolicitud.CAMBIO_CELULAR, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION
                ]
            },

            {
                TipoSolicitud.CORREO_EGRESADO, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION
                ]
            },

            {
                TipoSolicitud.CREACION_ACTIVACION_CORREO_INST, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION
                ]
            },

            {
                TipoSolicitud.RECUPERACION_CONTRA, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_INSCRIPCION
                ]
            },

            {
                TipoSolicitud.OTRO, 
                [
                    TipoDocumento.CURP, 
                    TipoDocumento.C_ERROR
                ]
            },

        };
    }
}
