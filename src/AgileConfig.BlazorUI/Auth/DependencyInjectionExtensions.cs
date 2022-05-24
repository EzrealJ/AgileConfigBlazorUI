using AgileConfig.BlazorUI.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

internal static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthorizationCore()
            .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>()
            .AddScoped<AuthService>()
            .AddScoped<IUserPermissionChecker, ApiAuthenticationStateProvider>()
            .AddSingleton<UIApiTokenProvider>();

        return services;
    }
}
