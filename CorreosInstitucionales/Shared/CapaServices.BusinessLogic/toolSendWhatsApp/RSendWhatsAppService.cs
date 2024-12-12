using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.toolSendWhatsApp
{
    public class RSendWhatsAppService(HttpClient client)
    {
        const string url = "https://www.developers.upiicsa.ipn.mx:8081/api/SendWhatsApp";
        HttpClient _client = client;
        JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<Response<string>> SendWhatsAppAsync(RequestDTO_SendWhatsApp oSendWhatsApp)
        {
            Response<string> oResponse = new() { Success = 0 };

            oSendWhatsApp.Number = (oSendWhatsApp.Number ?? "5500000000").Replace(" ", string.Empty);

            if (oSendWhatsApp.Number == "5500000000" || oSendWhatsApp.Number == "0000000000")
            {
                oResponse.Success = 1;
                oResponse.Data = "EL MENSAJE NO SE ENVIÓ DADO QUE ES UN NÚMERO DE PRUEBA.";
                return oResponse;
            }

            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(url, oSendWhatsApp, options: _options);

                if (response.IsSuccessStatusCode)
                    oResponse.Success = 1;

                oResponse.Data = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                oResponse.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }

            return oResponse;
        }
    }
}
