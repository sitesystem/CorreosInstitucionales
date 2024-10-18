using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                foreach (var prop in v.GetType().GetProperties())
                {
                    t.GetProperty(prop.Name)!.SetValue(result, prop.GetValue(v, null), null);
                }
            }

            return result;
        }

        /**************************  CREACION DE ELEMENTOS  *****************************/
        public static RequestDTO_Usuario DefaultUsuario()
        {
            return new RequestDTO_Usuario()
            {
                UsuIdRol = 2,
                UsuIdTipoPersonal = 1,
                UsuCurp = "XAXX010101HDFXXX00",
                UsuBoletaAlumno = "0000600000",
                UsuBoletaMaestria = "B000000",
                UsuCorreoPersonalCuentaNueva = "noreply@example.com",
                UsuContrasenia = Encrypt.GetSHA256(Guid.NewGuid().ToString()),
                UsuNoCelularNuevo = "55 00 00 00 00",
                UsuIdCarrera = 1,
                UsuSemestre = "1",
                UsuAnioEgreso = 1950,
                UsuFileNameComprobanteInscripcion = "-",
                UsuNumeroEmpleado = "0",
                UsuIdAreaDepto = 1,
                UsuNoExtension = "0",
                UsuStatus = true
            };
        }

        public static RequestDTO_Solicitud DefaultSolicitud()
        {
            return new RequestDTO_Solicitud()
            {
                SolFileNameCurp = "-",
                SolFileNameComprobanteInscripcion = "-",
                SolNoCelularAnterior = "-",
                SolNoCelularActual = "-",
                SolCorreoPersonalCuentaAnterior = "-",
                SolCorreoPersonalCuentaActual = "-",
                SolCapturaEscaneoAntivirus = "-",
                SolCapturaCuentaBloqueada = "-",
                SolCapturaError = "-",
                SolObservacionesSolicitud = "-",
                SolIdEstadoSolicitud = (int)TipoEstadoSolicitud.PENDIENTE
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
