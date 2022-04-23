using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using AgileConfig.UIApiClient.HttpResults;

namespace AgileConfig.BlazorUI.Auth
{
    public class AuthService
    {
        private readonly IAdminApi _adminApi;
        private readonly ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(
            IAdminApi adminApi,
            ILocalStorageService localStorageService,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _adminApi = adminApi;
            _localStorageService = localStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> LoginAsync(LoginVM rqtDto)
        {
            LoginResult rsp = await _adminApi.LoginAsync(rqtDto);
            if (rsp.Status!="ok")
            {
                return false;
            }
            await _localStorageService.SetItemAsync(Consts.Auth.AUTH_TOKEN_NAME, rsp);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(rqtDto.UserName);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync(Consts.Auth.AUTH_TOKEN_NAME);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }
}
