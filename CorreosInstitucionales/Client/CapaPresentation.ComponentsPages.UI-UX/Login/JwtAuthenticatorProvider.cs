using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace CorreosInstitucionales.Client.CapaPresentation.ComponentsPages.UI_UX.Login
{
    public class JwtAuthenticatorProvider : AuthenticationStateProvider, ILoginServices
    {
        private readonly IJSExtensions _jsRuntime;
        private readonly HttpClient _httpClient;
        public static readonly string TOKENKEY = "TokenKey";

        private static AuthenticationState _anonimo => new(new ClaimsPrincipal(new ClaimsIdentity()));

        public JwtAuthenticatorProvider(IJSRuntime jsRuntime, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsRuntime = new IJSExtensions(jsRuntime);
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _jsRuntime.GetFromLocalStorage(TOKENKEY);

            if (string.IsNullOrEmpty(token))
                return _anonimo;

            return BuildAuthenticationState(token);
        }

        public async Task Login(string token)
        {
            await _jsRuntime.RemoveItem(TOKENKEY);
            await _jsRuntime.SetInLocalStorage(TOKENKEY, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _jsRuntime.RemoveItem(TOKENKEY);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimo));
        }

        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
