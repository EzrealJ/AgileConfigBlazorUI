using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgileConfig.WindowsApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            ushort port = HostMachineHelper.GetTcpListenablePort(50000, 60000);
            var coreSettings = new CoreSettings
            {
                Uri = new Uri($"http://127.0.0.1:{port}")
            };
            List<string> argList = new();
            if (!args.Contains("--urls"))
            {
                argList.Add("--urls");
                argList.Add(coreSettings.Uri.ToString());
            }
            argList.AddRange(args);


            IHostBuilder hostBuilder = CreateHostBuilder(argList.ToArray());
            hostBuilder.ConfigureServices(services => services.AddSingleton(coreSettings));
            hostBuilder.UseWinForm<MainForm>().UseWinFormHostLifetime();
            IHost host = hostBuilder.Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => ConfigureCore(Host.CreateDefaultBuilder(args));

        public static IHostBuilder ConfigureCore(IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
