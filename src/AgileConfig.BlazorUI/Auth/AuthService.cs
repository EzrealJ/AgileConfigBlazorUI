using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace AgileConfig.BlazorUI.Auth
{
    public class AuthService
    {
        private readonly IAdminApi _adminApi;
        private readonly ApiAuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
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
        public async Task<TokenResult> GetAuthInfo() => await _localStorageService.GetItemAsync<TokenResult>(Consts.CacheKey.TOKEN);

        public List<string> GetAuthority() =>
            _syncLocalStorageService.GetItem<List<string>>(CacheKey.AUTHORITY);

        public async Task<List<string>> GetAuthorityAsync() =>
            await _localStorageService.GetItemAsync<List<string>>(CacheKey.AUTHORITY);

        public List<string> GetFunctions() =>
            _syncLocalStorageService.GetItem<List<string>>(CacheKey.FUNCTIONS);

        public async Task<List<string>> GetFunctionsAsync() =>
            await _localStorageService.GetItemAsync<List<string>>(CacheKey.FUNCTIONS);

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

        void NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
        {

        }
    }
}
