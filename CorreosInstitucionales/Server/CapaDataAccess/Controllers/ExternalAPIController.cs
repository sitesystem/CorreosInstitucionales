using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    public class ExternalAPIController: Controller
    {
        protected HttpClient Client;
        protected HttpClientHandler _client_handler;
        protected readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public ExternalAPIController()
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

    }
}
