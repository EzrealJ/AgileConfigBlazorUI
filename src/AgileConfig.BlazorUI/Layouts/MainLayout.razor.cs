using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Consts;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace AgileConfig.BlazorUI.Layouts
{
    public partial class MainLayout : LayoutComponentBase
    {
        private static MenuDataItem CreateMenuDataItem(
            string path,
            string name,
            string key,
            string icon) => new()
            {
                Path = path,
                Name = name,
                Key = key,
                Icon = icon,
            };

        private readonly MenuDataItem[] _menuData ={
        CreateMenuDataItem(RoutePath.HOME,"首页","home",""),
        CreateMenuDataItem(RoutePath.NODE,"节点","node",""),
        CreateMenuDataItem(RoutePath.APP,"应用","app",""),
        CreateMenuDataItem(RoutePath.CLIENT,"客户端","client",""),
        CreateMenuDataItem(RoutePath.SERVICE,"服务","service",""),
        CreateMenuDataItem(RoutePath.USER,"用户","user",""),
        CreateMenuDataItem(RoutePath.LOG,"日志","log",""),
    };
        [Inject] private UIApiClient.IHomeApi HomeApi { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private Auth.AuthService AuthService { get; set; }

        public string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
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
        }
    }
}
