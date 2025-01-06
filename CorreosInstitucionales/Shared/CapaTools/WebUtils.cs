using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;
using CorreosInstitucionales.Shared.Constantes;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public static class WebUtils
    {
        private static JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, WriteIndented = true };
        public static async Task<Response<T>?> GetResponse<T>(HttpResponseMessage message)
        {
            if(message.IsSuccessStatusCode)
            {
                return await message.Content.ReadFromJsonAsync< Response<T>>();
            }

            return new Response<T>() { Success = 0, Message = message.StatusCode.ToString() +Environment.NewLine + message.ReasonPhrase };
        }
        public static async Task<List<T>> ListByStatusAsync<T>(IGenericService<T> service, bool filterByStatus = true)
        {
            List<T> result = [];

            try
            {
                var response = await service.GetAllDataByStatusAsync(filterByStatus);

                if(response is not null && response.Data is not null)
                    result = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message} ");
            }

            return result;
        }

        public class Link(string url = "#", string name = "#", bool optional = false)
        {
            public string Url { get; set; } = url;
            public string Name { get; set; } = name;
            public bool Optional { get; set; } = optional;
        }

        public static string ArchivoSolicitud(RequestDTO_Solicitud solicitud, TipoDocumento tipo_documento)
        {
            string archivo = "ARCHIVO_404";

            switch (tipo_documento)
            {
                case TipoDocumento.CURP:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameCurp!;
                    break;

                case TipoDocumento.COM_INSCRIPCION:
                    archivo = solicitud.SolIdUsuarioNavigation.UsuFileNameComprobanteEstudios!;
                    break;

                case TipoDocumento.CAP_BLOQUEO:
                    archivo = solicitud.SolCapturaCuentaBloqueada!;
                    break;

                case TipoDocumento.CAP_ANTIVIRUS:
                    archivo = solicitud.SolCapturaEscaneoAntivirus!;
                    break;

                case TipoDocumento.CAP_ERROR:
                    archivo = solicitud.SolCapturaError!;
                    break;
            }

            return archivo;
        }

        public static string GenerarURLArchivoUsuario(RequestDTO_Solicitud solicitud, TipoDocumento tipo_documento)
        {
            string archivo = ArchivoSolicitud(solicitud, tipo_documento);
            return ServerFS.ArchivoUsuario(solicitud.SolIdUsuario, archivo);
        }

        public static string GenerarURLArchivoRepositorio(RequestDTO_Solicitud solicitud, TipoDocumento tipo_documento)
        {
            string archivo = ArchivoSolicitud(solicitud, tipo_documento);
            return ServerFS.ArchivoRepositorio(solicitud.IdSolicitudTicket, archivo);
        }

        public static string GenerarNombreArchivo(RequestDTO_Solicitud solicitud, TipoDocumento tipo_documento)
        {
            string archivo = ArchivoSolicitud(solicitud, tipo_documento);
            string ext = Path.GetExtension(archivo);
            string nombre = tipo_documento.GetNombre();

            return $"SOL{solicitud.IdSolicitudTicket}_{solicitud.SolIdUsuarioNavigation!.UsuCurp}_{nombre}{ext}";
        }
    }
}
