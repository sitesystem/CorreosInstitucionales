using CorreosInstitucionales.Shared.CapaEntities.Request;
using CorreosInstitucionales.Shared.CapaEntities.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CorreosInstitucionales.Shared.CapaTools
{
    public class WhatsApp(HttpClient client)
    {
        const string url = "https://www.developers.upiicsa.ipn.mx:8081/api/SendWhatsApp";

        public async Task<Response<string>> SendMessage(RequestDTO_SendWhatsApp message)
        {
            JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

            Response<string> oResponse = new();

            message.Number = message.Number.Replace(" ", string.Empty);

            if (message.Number == "5500000000")
            {
                oResponse.Success = 1;
                oResponse.Data = "EL MENSAJE NO SE ENVIÓ DADO QUE ES UN NÚMERO DE PRUEBA.";
                return oResponse;
            }

            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(url, message, options: _options);

                if (response.IsSuccessStatusCode)
                    oResponse.Success = 1;

                oResponse.Data = await response.Content.ReadAsStringAsync();

            }catch(Exception ex)
            {
                oResponse.Message = ex.Message + Environment.NewLine + ex.StackTrace;
            }
            

            return oResponse;
        }
    }
}
