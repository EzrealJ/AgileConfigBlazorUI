using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.UIApiClient;
using AntDesign;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Layouts
{
    public partial class MainLayout : LayoutComponentBase
    {
        private static MenuDataItem CreateMenuDataItem(
            string path,
            string name,
            string key,
            string icon,
            string[] authority = null) => new()
            {
                Path = path,
                Name = name,
                Key = key,
                Icon = icon,
                Authority = authority
            };

        private readonly MenuDataItem[] _menuData ={
        CreateMenuDataItem(RoutePath.HOME,"首页","home",""),
        CreateMenuDataItem(RoutePath.NODE,"节点","node",""),
        CreateMenuDataItem(RoutePath.APP,"应用","app",""),
        CreateMenuDataItem(RoutePath.CLIENT,"客户端","client",""),
        CreateMenuDataItem(RoutePath.SERVICE,"服务","service",""),
        CreateMenuDataItem(RoutePath.USER,"用户","user","",new []{ EnumRole.Admin.ToString()}),
        CreateMenuDataItem(RoutePath.LOG,"日志","log",""),
    };
        [Inject] private UIApiClient.IHomeApi HomeApi { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private MessageService MessageService { get; set; }
        [Inject] private Auth.AuthService AuthService { get; set; }

        public string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var temp = await HomeApi.CurrentAsync();
            UserName = temp?.CurrentUser?.UserName;
            StateHasChanged();
        }

        protected async Task OnUserItemSelected(AntDesign.MenuItem menuItem)
        {
            if (menuItem.Key == RoutePath.LOGOUT)
            {
                await AuthService.LogoutAsync();
                NavigationManager.NavigateTo(RoutePath.LOGIN);
            }
            if (menuItem.Key == RoutePath.RESET_PASSWORD)
            {
                NavigationManager.NavigateTo(RoutePath.RESET_PASSWORD);
            }
        }
        protected async Task OnLangItemSelected(AntDesign.MenuItem menuItem)
        {
            if (menuItem.Key == "en-US")
            {
                await MessageService.Info("Not completed~");
            }
        }

    }
}
