using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.Constantes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class EntityUtils
    {
        /*******************************  CONVERSIÓN  *******************************/
        public static T FromNavigation<T,V>(V v)
            where T : new()
        {
            T result = new();

            if(v != null)
            {
                Type t = result.GetType();
                PropertyInfo? pInfo;

                foreach (var prop in v.GetType().GetProperties())
                {
                    pInfo = t.GetProperty(prop.Name);

                    if(pInfo is not null)
                    {
                        pInfo.SetValue(result, prop.GetValue(v, null), null);
                    }
                }
            }

            return result;
        }

        /**************************  CREACION DE ELEMENTOS  *****************************/
        public static RequestDTO_Usuario DefaultUsuario()
        {
            return new RequestDTO_Usuario()
            {
                IdUsuario = 0,
                UsuIdRol = 2,
                UsuNombres = string.Empty,
                UsuPrimerApellido = string.Empty,
                UsuSegundoApellido = string.Empty,
                UsuIdTipoPersonal = 1,
                UsuCurp = "XAXX010101HDFXXX00",
                UsuFileNameCurp = "-",
                UsuNoCelularActual = "55 00 00 00 00",
                UsuBoletaAlumnoEgresado = "0000600000",
                UsuBoletaPosgrado = "B000000",
                UsuIdCarrera = 1,
                UsuSemestre = "1",
                UsuAnioEgreso = 1950,
                UsuFileNameComprobanteEstudios = "-",
                UsuNumeroEmpleadoContrato = "0",
                UsuIdAreaDepto = 1,
                UsuNoExtensionActual = "0",
                UsuNoExtensionAnterior = "0",
                UsuCorreoPersonalCuentaActual = "noreply@example.com",
                UsuContrasenia = Encrypt.GetSHA256(Guid.NewGuid().ToString()),
                UsuRecuperarContrasenia = false,
                UsuStatus = true,
                UsuIdRolNavigation = null,
                UsuIdTipoPersonalNavigation = null,
                UsuIdCarreraNavigation = null,
                UsuIdAreaDeptoNavigation = null
            };
        }

        public static RequestDTO_Solicitud DefaultSolicitud()
        {
            return new RequestDTO_Solicitud()
            {
                SolIdTipoSolicitud = 1,
                SolIdUsuario = 1,
                SolFileNameCurp = "-",
                SolFileNameComprobanteInscripcion = "-",
                SolNoCelularAnterior = "55 00 00 00 00",
                SolNoCelularActual = "55 00 00 00 00",
                SolCorreoPersonalCuentaAnterior = "noreply@example.com",
                SolCorreoPersonalCuentaActual = "noreply@example.com",
                SolCapturaEscaneoAntivirus = "-",
                SolCapturaCuentaBloqueada = "-",
                SolCapturaError = "-",
                SolObservacionesSolicitud = "-",
                SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.PENDIENTE,
                SolValidacionDatos = false,
                SolEnvioEncuesta = 0,
                SolIdUsuarioNavigation = null,
                SolIdTipoSolicitudNavigation = null,
                SolIdEstadoSolicitudNavigation = null
            };
        }

        public static RequestViewModel_Anuncio DefaultAnuncio()
        {
            return new RequestViewModel_Anuncio()
            {
                AnuArchivo = "-",
                AnuStatus = true
            };
        }
    }
}
