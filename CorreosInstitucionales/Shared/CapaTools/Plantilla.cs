using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using CorreosInstitucionales.Shared.CapaEntities.Request;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class Plantilla(IEnumerable<RequestViewModel_Plantilla> plantillas)
    {
        const int PLANTILLA_CORREO = 1;
        const int PLANTILLA_WA = 2;

        IEnumerable<RequestViewModel_Plantilla> _plantillas = plantillas;

        public KeyValuePair<string,string> GetDictonaryEntry(object obj, PropertyInfo p)
        {
            object? obj_value = p.GetValue(obj, null);

            return new KeyValuePair<string, string>
            (
                p.Name, 
                obj_value is null ? string.Empty : (string)obj_value
            );
        }

        public Notificacion Generar(RequestDTO_Solicitud solicitud)
        {
            Notificacion notificacion = new();

            int filtro_plantilla = solicitud.Usuario!.EsAlumnoOEgresado() ? 0 : 1;

            RequestViewModel_Plantilla? plantilla_correo = _plantillas
                .Where(p =>
                    p.PlaIdEstadoSolicitud == solicitud.SolIdEstadoSolicitud &&
                    p.PlaTipo == PLANTILLA_CORREO &&
                    p.PlaFiltro == filtro_plantilla
                )
                .FirstOrDefault();

            RequestViewModel_Plantilla? plantilla_wa = _plantillas
                .Where(p =>
                    p.PlaIdEstadoSolicitud == solicitud.SolIdEstadoSolicitud &&
                    p.PlaTipo == PLANTILLA_WA
                )
                .FirstOrDefault();

            StringBuilder sb;
            Dictionary<string,string> datos_solicitud = solicitud.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select
                (
                    p => GetDictonaryEntry(solicitud, p) 
                )
                .ToDictionary();

            Dictionary<string, string> datos_usuario = solicitud.Usuario.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select
                (
                    p => GetDictonaryEntry(solicitud, p)
                )
                .ToDictionary();

            if (plantilla_correo is not null)
            {
                sb = new StringBuilder(plantilla_correo.PlaContenido);

                foreach(KeyValuePair<string,string> dato in datos_solicitud)
                {
                    sb.Replace($"{{solicitud.{dato.Key}}}", dato.Value);
                }

                foreach (KeyValuePair<string, string> dato in datos_usuario)
                {
                    sb.Replace($"{{usuario.{dato.Key}}}", dato.Value);
                }

                notificacion.correo.Subject = plantilla_correo.PlaAsunto;
                notificacion.correo.EmailTo = solicitud.Usuario.UsuCorreoPersonalCuentaActual!;
                notificacion.correo.Body = sb.ToString();
            }

            if (plantilla_wa is not null)
            {
                sb = new StringBuilder(plantilla_wa.PlaContenido);

                foreach (KeyValuePair<string, string> dato in datos_solicitud)
                {
                    sb.Replace($"{{solicitud.{dato.Key}}}", dato.Value);
                }

                foreach (KeyValuePair<string, string> dato in datos_usuario)
                {
                    sb.Replace($"{{usuario.{dato.Key}}}", dato.Value);
                }

                notificacion.wa.Number = solicitud.Usuario.UsuNoCelularActual;
                notificacion.wa.Message = sb.ToString();
            }

            return notificacion;
        }
    }
}
