using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AntDesign.ProLayout;
using Blazor.Extensions.Logging;
using Blazored.LocalStorage;
using HighlightBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AgileConfig.BlazorUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            IServiceCollection services = builder.Services;
            ConfigureServices(builder, services);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(WebAssemblyHostBuilder builder, IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddBrowserConsole().SetMinimumLevel(LogLevel.Warning);
            });
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddAntDesign();
            services.Configure<ProSettings>(builder.Configuration.GetSection("ProSettings"));

            services.AddAgileConfigUIApiClient((option, sp) =>
            {
                IConfiguration configuration = sp.GetRequiredService<IConfiguration>();
                string server = configuration["AgileConfigServer"];
                option.HttpHost = new Uri(server);
                UIApiTokenProvider filter = sp.GetRequiredService<UIApiTokenProvider>();
                option.GlobalFilters.Add(filter);
            });
            services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                config.JsonSerializerOptions.WriteIndented = false;
            });
            services.AddAuth();
            services.AddHighlight();
        }
    }
}
