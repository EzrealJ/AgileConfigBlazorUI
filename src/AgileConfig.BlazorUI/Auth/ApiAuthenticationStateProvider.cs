using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Linq;
using AgileConfig.UIApiClient.HttpResults;

namespace AgileConfig.BlazorUI.Auth
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }
       
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authInfo = await _localStorage.GetItemAsync<LoginResult>(Consts.Auth.AUTH_TOKEN_NAME);

            if (string.IsNullOrWhiteSpace(authInfo.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(authInfo.Token), "jwt")));
        }

        public void MarkUserAsAuthenticated(string account)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, account) }, "apiauth"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles) && roles is string rolesText)
            {
                if (rolesText.StartsWith('['))
                {
                    string[] parsedRoles = JsonSerializer.Deserialize<string[]>(rolesText);
                    foreach (string parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, rolesText));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
