using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore;

namespace AgileConfig.BlazorUI.Filters
{
    public class AgileConfigServerFilter : IApiFilter
    {
        public async Task OnRequestAsync(ApiRequestContext context)
        {
            var sp = context.HttpContext.ServiceProvider;
            AgileConfigServerProvider agileConfigServerProvider = sp.GetRequiredService<AgileConfigServerProvider>();

            var serverSetting = await agileConfigServerProvider.GetCurrentAsync();
            if (serverSetting != default)
            {
                HttpApiRequestMessage requestMessage = context.HttpContext.RequestMessage;
                requestMessage.RequestUri = requestMessage.MakeRequestUri(new Uri(serverSetting.Url));
            }
            ApiAuthenticationStateProvider apiAuthenticationStateProvider = sp.GetRequiredService<AuthenticationStateProvider>() as ApiAuthenticationStateProvider;
            var authInfo = await apiAuthenticationStateProvider.GetLoginResultAsync();
            if (authInfo != null)
            {
                context.HttpContext.RequestMessage.Headers.Authorization = new AuthenticationHeaderValue(authInfo.Type, authInfo.Token);
            }
        }

        public async Task OnResponseAsync(ApiResponseContext context)
        {
            var sp = context.HttpContext.ServiceProvider;
            if (context.HttpContext?.ResponseMessage?.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var stateProvider = sp.GetRequiredService<AuthenticationStateProvider>() as ApiAuthenticationStateProvider;
                stateProvider.MarkUserAsLoggedOut();
            }
            await ValueTask.CompletedTask;
        }
    }
}
