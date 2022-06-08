using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Components.Config
{
    public partial class ConfigItemHistory
    {
        public bool Visible = false;
        readonly string _placement = "right";
        private List<ConfigItemPublishedLog> _dataSource = new(0);
        [Inject]
        public IConfigApi ConfigApi { get; set; }

        [Parameter]
        public ConfigItemHistoryPara Para { get; set; } = new();

        private ConfigVM Config => Para?.Config;
        private string Title => Config?.AppId + "配置项" + Config?.Key + "的历史记录";
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            ApiResult<List<ConfigItemPublishedLog>> res = await ConfigApi.ConfigPublishedHistoryAsync(Config.Id, Para.ENV);
            _dataSource = res?.Data;
        }

        void Close()
        {
            Para = new();
            this.Visible = false;
        }
    }

    public class ConfigItemHistoryPara
    {
        public ConfigVM Config { get; set; } = new();
        public string ENV { get; set; }
    }
}
