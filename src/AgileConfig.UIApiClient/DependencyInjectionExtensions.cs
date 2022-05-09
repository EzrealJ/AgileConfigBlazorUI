using AgileConfig.UIApiClient;
using WebApiClientCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 添加AgileConfig的OpenApiClient
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAgileConfigUIApiClient(this IServiceCollection services, Action<HttpApiOptions, IServiceProvider> configureOptions)
        {
            services.AddHttpApi<IAdminApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IAppApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IConfigApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IHomeApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IRemoteOPApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IRemoteServerProxyApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IReportApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IServerNodeApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<ISysLogApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IUserApi>().ConfigureHttpApi(configureOptions);
            services.AddHttpApi<IServiceApi>().ConfigureHttpApi(configureOptions);
            return services;
        }
    }
}
