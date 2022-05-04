using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigItemHistoryPara
    {
        public ConfigVM Config { get; set; } = new();
        public string ENV { get; set; }
    }
    public partial class ConfigItemHistory
    {
        readonly string _placement = "right";
        private string Title => Config?.AppId + "配置项" + Config?.Key + "的历史记录";

        public bool Visible = false;
        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Parameter]
        public ConfigItemHistoryPara Para { get; set; } = new();

        private ConfigVM Config => Para?.Config;

        private List<ConfigItemPublishedLog> _dataSource = new(0);
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
}
