using AgileConfig.OpenApiClient;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// 添加AgileConfig的OpenApiClient
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAgileConfigOpenApiClient(this IServiceCollection services)
        {
            services.AddHttpApi<IAppApi>();
            services.AddHttpApi<IConfigApi>();
            services.AddHttpApi<INodeApi>();
            services.AddHttpApi<IRegisterCenterApi>();
            return services;
        }
    }
}
