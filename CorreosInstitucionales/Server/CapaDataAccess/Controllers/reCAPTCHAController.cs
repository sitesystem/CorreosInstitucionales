using CorreosInstitucionales.Shared.CapaTools.reCAPTCHA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CorreosInstitucionales.Server.CapaDataAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reCAPTCHAController : ControllerBase
    {
        private IHttpClientFactory HttpClientFactory { get; }

        private IOptions<reCAPTCHAVerificationOptions> reCAPTCHAVerificationOptions { get; }

        public reCAPTCHAController(IHttpClientFactory httpClientFactory, IOptions<reCAPTCHAVerificationOptions> reCAPTCHAVerificationOptions)
        {
            this.HttpClientFactory = httpClientFactory;
            this.reCAPTCHAVerificationOptions = reCAPTCHAVerificationOptions;
        }

        public async Task<IActionResult> Post([FromBody] SampleAPIArgs args)
        {
            var url = "https://www.google.com/recaptcha/api/siteverify";
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"secret", this.reCAPTCHAVerificationOptions.Value.Secret},
                {"response", args.reCAPTCHAResponse}
            });

            var httpClient = this.HttpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var verificationResponse = await response.Content.ReadFromJsonAsync<reCAPTCHAVerificationResponse>();
            if (verificationResponse.Success) return Ok();

            return BadRequest(string.Join(", ", verificationResponse.ErrorCodes.Select(err => err.Replace('-', ' '))));
        }
    }
}
