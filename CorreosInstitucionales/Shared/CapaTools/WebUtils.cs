using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaServices.BusinessLogic;

namespace CorreosInstitucionales.Shared.Utils
{
    public static class WebUtils
    {
        static public HttpClient Client;
        static HttpClientHandler _client_handler;

        static WebUtils()
        {
            _client_handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
            };

            Client = new HttpClient(_client_handler);
        }

        public static async Task<List<T>> ListByStatusAsync<T>(IGenericService<T> service, bool filterByStatus = true)
        {
            List<T> result = new();

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

        public class Link(string url = "#", string name = "#")
        {
            public string Url { get; set; } = url;
            public string Name { get; set; } = name;
        }
    }
}
