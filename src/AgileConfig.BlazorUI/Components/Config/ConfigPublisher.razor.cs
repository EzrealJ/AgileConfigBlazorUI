using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigPublisherParameter
    {
        public string AppId { get; set; }
        public string ENV { get; set; }
    }
    public partial class ConfigPublisher
    {
        public bool Visible { get; set; }

        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        [Parameter]
        public EventCallback OnCompleted { get; set; }
        [Parameter]
        public ConfigPublisherParameter Para { get; set; } = new();

        private string _log;

        private void Cancel(MouseEventArgs e)
        {
            _log = string.Empty;
            Visible = false;
        }

        private async Task OnOkAsync(MouseEventArgs e)
        {
            var config = new MessageConfig()
            {
                Content = "发布中...",
                Key = $"{nameof(ConfigImport)}-{Para.AppId}"
            };
            var res = await ConfigApi.PublishAsync(Para.ENV, new PublishLogVM
            {
                AppId = Para.AppId,
                Log = _log
            });
            if (res.Success)
            {
                config.Content = "发布成功";
                await MessageService.Success(config);
                Visible = false;
            }
            else
            {
                config.Content = $"发布失败,{res.Message}";
                await MessageService.Error(config);
            }
            _ = OnCompleted.InvokeAsync();

        }
    }
}
