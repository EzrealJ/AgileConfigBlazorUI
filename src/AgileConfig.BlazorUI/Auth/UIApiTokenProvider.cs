using System.Net.Http.Headers;
using System.Threading.Tasks;
using AgileConfig.UIApiClient.HttpResults;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore;

namespace AgileConfig.BlazorUI.Auth
{
    public class UIApiTokenProvider : IApiFilter
    {


        public async Task OnRequestAsync(ApiRequestContext context)
        {
            var sp = context.HttpContext.ServiceProvider;
            ILocalStorageService localStorageService = sp.GetRequiredService<ILocalStorageService>();
            var authInfo = await localStorageService.GetItemAsync<LoginResult>(Consts.Auth.AUTH_TOKEN_NAME);
            if (authInfo != null)
            {
                context.HttpContext.RequestMessage.Headers.Authorization = new AuthenticationHeaderValue(authInfo.Type, authInfo.Token);
            }
        }

        public async Task OnResponseAsync(ApiResponseContext context)
        {
            var sp = context.HttpContext.ServiceProvider;
            if (context.HttpContext.ResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var stateProvider = sp.GetRequiredService<AuthenticationStateProvider>() as ApiAuthenticationStateProvider;
                stateProvider.MarkUserAsLoggedOut();
            }
            await Task.CompletedTask;
        }
    }
}
