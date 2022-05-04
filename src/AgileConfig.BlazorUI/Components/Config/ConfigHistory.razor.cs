using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigHistoryParameter
    {
        public string AppId { get; set; }
        public string ENV { get; set; }
    }
    public partial class ConfigHistory
    {
        [Parameter]
        public ConfigHistoryParameter Para { get; set; } = new();
        public bool Visible { get; set; }
        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        private List<ConfigPublishedLog> _dataSource = new(0);
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            var res = await ConfigApi.PublishHistoryAsync(Para.AppId, Para.ENV);
            _dataSource = res.Data;
        }
        private static string GetItemTitle(PublishTimeline timeline)
            => $"{timeline?.PublishTime?.ToString(Consts.Format.DATE_TIME_YYYY_MM_DD_HH_MM_SS)}/{timeline?.PublishUserName}";

        private void Close()
        {
            Para = new();
            Visible = false;
        }
        private async Task RollBackAsync()
        {
            await MessageService.Info("你点击了回滚");
            await Task.CompletedTask;
        }
    }
}
