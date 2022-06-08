using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AgileConfig.UIApiClient.HttpResults;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AgileConfig.BlazorUI.Auth
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider, IUserPermissionChecker
    {
        private readonly ILocalStorageService _localStorage;

        public ApiAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public bool CheckUserPermission(IEnumerable<string> functions, string judgeKey, string appId)
        {
            appId ??= string.Empty;
            string matchKey = $"GLOBAL_{judgeKey}";
            bool ex = functions.Any(x => x == matchKey);
            if (ex)
            {
                return true;
            }
            matchKey = $"APP_{appId}_{judgeKey}";
            ex = functions.Any(x => x == matchKey);
            return ex;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var authInfo = await _localStorage.GetItemAsync<LoginResult>(Consts.CacheKey.TOKEN);

            if (string.IsNullOrWhiteSpace(authInfo?.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var user = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(authInfo.Token), "jwt"));
            return new AuthenticationState(user);
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

        public void NotifyStateChange()
        {
            var authState = GetAuthenticationStateAsync();
            NotifyAuthenticationStateChanged(authState);
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
    }
}
