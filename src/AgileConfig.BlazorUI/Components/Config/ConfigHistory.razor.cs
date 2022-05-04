using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Pages;
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
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        public bool Visible { get; set; }
        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public ModalService ModalService { get; set; }

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
        private void RollBackConfirm(PublishTimeline timeline)
        {
            var options = new ConfirmOptions()
            {
                Title = $"确定回滚至【{timeline?.PublishTime?.ToString(Consts.Format.DATE_TIME_YYYY_MM_DD_HH_MM_SS)}】时刻的发布版本吗？",
                Icon = infoIcon,
                OnOk = async e => await RollBackAsync(timeline)
            };
            ModalService.Confirm(options);
        }
        private async Task RollBackAsync(PublishTimeline timeline)
        {
            var config = new MessageConfig()
            {
                Content = "回滚中...",
                Key = $"{nameof(RollBackAsync)}-{Para.AppId}"
            };
            _ = MessageService.Loading(config);
            var res = await ConfigApi.RollbackAsync(timeline.Id, Para.ENV);
            if (res.Success)
            {
                config.Content = "回滚成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"回滚失败,{res.Message}";
                await MessageService.Error(config);
            }
            await OnCompleted.InvokeAsync();
        }
    }
}
