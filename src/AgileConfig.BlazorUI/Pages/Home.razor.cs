using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Home : IDisposable
    {
        #region UI Bind
        readonly string _iconTheme = IconHelper.IconThemes.Outline;
        protected (Dictionary<string, int> X, int Y) Gutter => (_gutterX, _gutterY);
        protected string NodeValue => $"{_nodeStatuses.Count(s => s.N.Status == NodeStatus.Online)}/{_nodeStatuses.Count}";
        protected int AppCount { get; set; }
        protected int ConfigCount { get; set; }
        protected int ClientCount => _nodeStatuses.Sum(s => s?.ServerStatus?.ClientCount ?? 0);
        protected string ServiceValue => $"{_serviceCount.ServiceOnCount}/{_serviceCount.ServiceCount}";

        #endregion
        ServiceCountResult _serviceCount = new();
        List<RemoteNodeStatus> _nodeStatuses = new();
        static readonly int _gutterY = 24;
        static readonly Dictionary<string, int> _gutterX = new()
        {
            ["xs"] = 8,
            ["sm"] = 16,
            ["md"] = 24,
            ["lg"] = 32,
            ["xl"] = 48,
            ["xxl"] = 64
        };
        [Inject]
        private IReportApi ReportApi { get; set; }
        PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(3));
        protected override Task OnInitializedAsync()
        {
            _ = RunTimerAsync();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _periodicTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
        private async Task RunTimerAsync()
        {
            do
            {
                await ReloadAsync();
                StateHasChanged();
            } while (await _periodicTimer.WaitForNextTickAsync());
        }

        private async Task ReloadAsync()
        {
            _nodeStatuses = await ReportApi.RemoteNodesStatusAsync();
            AppCount = await ReportApi.AppCountAsync();
            ConfigCount = await ReportApi.ConfigCountAsync();
            _serviceCount = await ReportApi.ServiceCountAsync();
            Console.WriteLine("Reload Done");
            return;
        }

        private static RemoteNodeStatus CreateTestData(Random rd)
        {
            return new RemoteNodeStatus
            {
                N = new()
                {
                    CreateTime = DateTime.Now,
                    LastEchoTime = DateTime.Now.AddSeconds(-1 * rd.Next(0, 10)),
                    Status = rd.Next(0, 100) > 50 ? NodeStatus.Online : NodeStatus.Offline,
                },
                ServerStatus = new ServerStatus
                {
                    ClientCount = rd.Next(0, 10),
                    Infos = new ServerInfo[]
                                   {
                           new ServerInfo{
                           AppId=Guid.NewGuid().ToString(),
                           LastHeartbeatTime=DateTime.Now,
                           Name=Guid.NewGuid().ToString()[..6]
                           }
                    }
                }
            };
        }


        [Inject]
        public NavigationManager NavigationManager1 { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            NavigationManager1.NavigateTo("/user");
        }
    }
}
