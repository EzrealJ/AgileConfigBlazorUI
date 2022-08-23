using System.Threading.Tasks;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.BlazorUI.Services;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class InitPassword
    {
        private Form<InitPasswordVM> _form;
        private InitPasswordVM _initPasswordVM = new();
        [Inject]
        public IAdminApi AdminApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public NavigationService NavigationService { get; set; }

        private async Task SubmitAsync()
        {
            if (!_form.Validate())
            {
                return;
            }

            var config = new MessageConfig()
            {
                Content = "初始化中...",
                Key = $"{nameof(ResetPassword)}-{nameof(SubmitAsync)}"
            };
            _ = MessageService.Loading(config);
            var res = await AdminApi.InitPasswordAsync(_initPasswordVM);
            if (res.Success)
            {
                config.Content = "初始化成功";
                await MessageService.Success(config);
                NavigationService.NavigateTo(RoutePath.LOGIN);
            }
            else
            {
                config.Content = $"初始化失败,{res.Message}";
                await MessageService.Error(config);
            }
        }
    }
}
