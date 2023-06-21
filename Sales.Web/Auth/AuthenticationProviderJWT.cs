using Sales.Web.Helpers;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;

namespace Sales.Web.Auth
{
    public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
    {
        private readonly string _tokenKey;
        private readonly IJSRuntime _jSRuntime;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationState _anonimous;

        public AuthenticationProviderJWT(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            _tokenKey = "TokenKey";
            _jSRuntime = jSRuntime;
            _httpClient = httpClient;
        }

        public async Task LoginAsync(string token)
        {
            await _jSRuntime.SetLocalStorage(_tokenKey, token);
            AuthenticationState authState = BuilderAuthentiocationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task LogoutAsync()
        {
            await _jSRuntime.RemoveLocalStorage(_tokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));
        }

        public async Task<bool> IstokenActive()
        {
            object token = await _jSRuntime.GetLocalStorage(_tokenKey);

            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken jwtSecurity = tokenHandler.ReadJwtToken(token.ToString());
            DateTime expire = jwtSecurity.ValidTo;

            if (DateTime.UtcNow > expire)
            {
                await LogoutAsync();
                return false;
            }
            return true;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            object? token = await _jSRuntime.GetLocalStorage(_tokenKey);
            if (token is null)
                return _anonimous;

            return BuilderAuthentiocationState(token.ToString()!);
        }

        private AuthenticationState BuilderAuthentiocationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            IEnumerable<Claim> claims = ParseClaimsFromJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private static IEnumerable<Claim> ParseClaimsFromJWT(string token)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            JwtSecurityToken unserializeToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            return unserializeToken.Claims;
        }
    }
}
