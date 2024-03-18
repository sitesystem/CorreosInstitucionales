using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class TemplateEntity
    {
        public static RequestDTO_Usuario CreateUsuario()
        {
            return new RequestDTO_Usuario()
            {
                UsuBoletaAlumno = "0000600000",
                UsuBoletaMaestria = "B000000",
                UsuIdCarrera = 1,
                UsuSemestre = "0",
                UsuAñoEgreso = 1950,
                UsuFileNameComprobanteInscripcion = "-",
                UsuNumeroEmpleado = "0",
                UsuIdAreaDepto = 1,
                UsuNoExtension = "0",
                UsuStatus = true
            };
        }

        public static RequestDTO_Solicitud CreateSolicitud()
        {
            return new RequestDTO_Solicitud()
            {
                SolFileNameCurp = "-",
                SolFileNameComprobanteInscripcion = "-",
                SolNoCelularNuevo = "-",
                SolCorreoPersonalCuentaNueva = "-",
                SolCapturaEscaneoAntivirus = "-",
                SolCapturaCuentaBloqueada = "-",
                SolCapturaError = "-",
                SolObservacionesSolicitud = "-",
                SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.PENDIENTE
            };
        }


    }
}
