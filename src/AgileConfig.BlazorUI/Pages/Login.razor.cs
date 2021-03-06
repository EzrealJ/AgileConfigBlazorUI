using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Login
    {
        private LoginVM _loginData = new();
        [Inject]
        public IAdminApi AdminApi { get; set; }

        [Inject]
        public AuthService AuthService { get; set; }

        [Inject]
        public MessageService MsgService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var res = await AdminApi.PasswordInitedAsync();
            if (res.Data)
            {
                return;
            }
            NavigationManager.NavigateTo(RoutePath.INIT_PASSWORD);
        }


        private async Task OnFinish(EditContext editContext)
        {
            var result = await AuthService.LoginAsync(_loginData);
            if (!result)
            {
                await MsgService.Error("帐号或密码错误！");
                return;
            }
            await MsgService.Success("登录成功！");
            NavigationManager.NavigateTo("/home");
        }
    }
}
