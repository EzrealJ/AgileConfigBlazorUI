using AgileConfig.OpenApiClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
