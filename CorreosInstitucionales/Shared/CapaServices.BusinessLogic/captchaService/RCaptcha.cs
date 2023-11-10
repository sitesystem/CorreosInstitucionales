using CorreosInstitucionales.Shared.CapaTools;
using System.Text.Json.Serialization;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Request;
using CorreosInstitucionales.Shared.CapaEntities.ViewModels.Response;
using System.Text.Json;

namespace CorreosInstitucionales.Shared.CapaServices.BusinessLogic.captchaService
{
    public class RCaptcha : IReCaptcha
    {
        public async Task<bool> ValidateResponse(string recaptchaResponse, string secretKey)
        {
            var validResponse = false;

            if (!string.IsNullOrEmpty(secretKey)&& !string.IsNullOrEmpty(recaptchaResponse))
            {
                var googleUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponse}";

                using var httpClient =new HttpClient(); 
                var request =new HttpRequestMessage(HttpMethod.Post, googleUrl);
                var response=httpClient.SendAsync(request);

                if (response.Result.StatusCode==System.Net.HttpStatusCode.OK)
                {
                    var responseString = await response.Result.Content.ReadAsStringAsync();
                    var reCaptchaResponse = JsonSerializer.Deserialize<ReCaptchaResponse>(responseString,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    if (reCaptchaResponse!=null)
                    {
                        validResponse = reCaptchaResponse.Success;
                    }
                }
            }
            return validResponse;
        }
    }
}
