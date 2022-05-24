using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using AgileConfig.UIApiClient.HttpResults;
using AgileConfig.BlazorUI.Consts;
using System.Security.Cryptography;
using AgileConfig.BlazorUI.Extensions;

namespace AgileConfig.BlazorUI.Auth
{
    public class AuthService
    {
        private readonly IAdminApi _adminApi;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly ApiAuthenticationStateProvider _authenticationStateProvider;

        public AuthService(
            IAdminApi adminApi,
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _adminApi = adminApi;
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            _authenticationStateProvider = (ApiAuthenticationStateProvider)authenticationStateProvider;
        }
        void NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
        {

        }
        public async Task<bool> LoginAsync(LoginVM rqtDto)
        {
            LoginResult rsp = await _adminApi.LoginAsync(rqtDto);
            if (rsp.Status != "ok")
            {
                return false;
            }
            await _localStorageService.SetItemAsync<TokenResult>(CacheKey.TOKEN, rsp);
            await SetAuthorityAsync(rsp.CurrentAuthority);
            await SetFunctionsAsync(rsp.CurrentFunctions);
            _authenticationStateProvider.MarkUserAsAuthenticated(rqtDto.UserName);
            return true;
        }

        public async Task<TokenResult> GetAuthInfo() => await _localStorageService.GetItemAsync<TokenResult>(Consts.CacheKey.TOKEN);

        public async Task LogoutAsync()
        {
            await _localStorageService.RemoveItemAsync(CacheKey.TOKEN);
            await SetAuthorityAsync(null);
            await SetFunctionsAsync(null);
            _authenticationStateProvider.MarkUserAsLoggedOut();
        }
        public async Task SetAuthorityAsync(IEnumerable<string> authority)
        {
            await _localStorageService.SetItemAsync(CacheKey.AUTHORITY, authority);
            _authenticationStateProvider.NotifyStateChange();
        }
        public async Task SetFunctionsAsync(IEnumerable<string> functions)
        {
            await _localStorageService.SetItemAsync(CacheKey.FUNCTIONS, functions);
            _authenticationStateProvider.NotifyStateChange();
        }
        public async Task<List<string>> GetAuthorityAsync() =>
            await _localStorageService.GetItemAsync<List<string>>(CacheKey.AUTHORITY);
        public async Task<List<string>> GetFunctionsAsync() =>
            await _localStorageService.GetItemAsync<List<string>>(CacheKey.FUNCTIONS);

        public List<string> GetAuthority() =>
            _syncLocalStorageService.GetItem<List<string>>(CacheKey.AUTHORITY);
        public List<string> GetFunctions() =>
            _syncLocalStorageService.GetItem<List<string>>(CacheKey.FUNCTIONS);

    }
}
