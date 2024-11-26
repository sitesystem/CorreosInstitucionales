using System.Text;
using System.Reflection;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaDataAccess.DBContext;
using CorreosInstitucionales.Shared.Constantes;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using System.Collections;
using CorreosInstitucionales.Shared.CapaDataAccess;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class PlantillaManager()
    {
        public const int PLANTILLA_CORREO = 1;
        public const int PLANTILLA_WA = 2;

        public const int FILTRO_NINGUNO = 0;
        public const int FILTRO_EMPLEADO = 1;
        public const int FILTRO_RECORDATORIO = 2;
        public const int FILTRO_RECUPERACION_CONTRA = 3;
        public const int FILTRO_ERROR_ALTA_USUARIO = 4;
        public const int FILTRO_ALTA_USUARIO = 5;
        public const int FILTRO_EDICION_USUARIO = 6;

        public static Response<Notificacion?> GetNotificacion(Dictionary<string, object?> datos, int estado, int filtro = 0)
        {
            Response<Notificacion?> response = new Response<Notificacion?>() { Success = 0 };

            StringBuilder log = new();

            McCatPlantillas? plantilla_correo = GetPlantilla(estado, PLANTILLA_CORREO, filtro);
            McCatPlantillas? plantilla_wa = GetPlantilla(estado, PLANTILLA_WA, filtro);


            if (plantilla_correo is null)
            {
                log.AppendLine($"[ERROR] PLANTILLA DE CORREO NO ENCONTRADA: ESTADO = {estado}, FILTRO = {filtro}");
            }

            if (plantilla_wa is null)
            {
                log.AppendLine($"[ERROR] PLANTILLA DE WA NO ENCONTRADA: ESTADO = {estado}, FILTRO = {filtro}");
            }

            if (plantilla_correo is null || plantilla_wa is null)
            {
                response.Message = log.ToString();
                return response;
            }

            string contenido_correo = LLenar(plantilla_correo!.PlaContenido, datos);
            string contenido_wa = LLenar(plantilla_wa!.PlaContenido, datos);

            response.Data = new()
            {
                correo = new RequestDTO_SendEmail()
                {
                    Subject = plantilla_correo.PlaAsunto,
                    Body = contenido_correo
                },
                wa = new RequestDTO_SendWhatsApp()
                {
                    Message = contenido_wa
                }
            };

            response.Success = 1;

            return response;
        }

        public static string LLenar(string plantilla, Dictionary<string,object?> objetos)
        {
            StringBuilder sb = new StringBuilder(plantilla);
            Dictionary<string, object?> propiedades = new();

            foreach (KeyValuePair<string,object?> objeto in objetos)
            {
                if (objeto.Value is null)
                {
                    continue;
                }

                //https://stackoverflow.com/a/17190236/3930332

                if(objeto.Value is IDictionary<string,object?>)
                {
                    propiedades = (Dictionary<string, object?>)objeto.Value!;
                }
                else
                {
                    propiedades = objeto.Value.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(
                        p => p.Name, p => p.GetValue(objeto.Value)
                    );
                }
                
                foreach(KeyValuePair<string,object?> propiedad in propiedades)
                {
                    sb.Replace
                    (
                        $"{{{objeto.Key}.{propiedad.Key}}}", 
                        propiedad.Value == null ? string.Empty : propiedad.Value.ToString()!.Trim()
                    );
                }
            }

            return sb.ToString();
        }

        public static McCatPlantillas? GetPlantilla(
            int estado_solicitud, 
            int tipo_plantilla = PLANTILLA_CORREO,
            int filtro = 0)
        {
            McCatPlantillas[] plantillas = AppCache.Plantillas
               .Where(p =>
                   p.PlaIdEstadoSolicitud == estado_solicitud &&
                   p.PlaTipo == tipo_plantilla
               ).ToArray();

            if(plantillas.Length == 1)
            {
                return plantillas[0];
            }
            
            return plantillas.Where(p => p.PlaFiltro == filtro).First();
        }
    }
}
