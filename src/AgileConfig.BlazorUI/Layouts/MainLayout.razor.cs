using System;
using System.Collections.Generic;
using System.Linq;
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

        private MenuDataItem[] _menuData = Array.Empty<MenuDataItem>();
        [Inject] private UIApiClient.IHomeApi HomeApi { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private MessageService MessageService { get; set; }
        [Inject] private AuthService AuthService { get; set; }

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
            SetMenus();
            StateHasChanged();
        }

        private void SetMenus()
        {
            List<MenuDataItem> list = new();

            var home = CreateMenuDataItem(RoutePath.HOME, "首页", "home", "");
            var node = CreateMenuDataItem(RoutePath.NODE, "节点", "node", "");
            var app = CreateMenuDataItem(RoutePath.APP, "应用", "app", "");
            var client = CreateMenuDataItem(RoutePath.CLIENT, "客户端", "client", "");
            var service = CreateMenuDataItem(RoutePath.SERVICE, "服务", "service", "");
            var user = CreateMenuDataItem(RoutePath.USER, "用户", "user", "", new[] { EnumRole.Admin.ToString() });
            var log = CreateMenuDataItem(RoutePath.LOG, "日志", "log", "");
            list.Add(home);
            list.Add(node);
            list.Add(app);
            list.Add(client);
            list.Add(service);
            if (AuthService.GetAuthority()?.Select(a => Enum.Parse<EnumRole>(a)).Contains(EnumRole.Admin) ?? false)
            {
                list.Add(user);
            }
            list.Add(log);
            _menuData = list.ToArray();
        }

        protected async Task OnUserItemSelected(AntDesign.MenuItem menuItem)
        {
            if (menuItem.Key == RoutePath.LOGOUT)
            {
                await Logout();
            }
            if (menuItem.Key == RoutePath.RESET_PASSWORD)
            {
                NavigationManager.NavigateTo(RoutePath.RESET_PASSWORD);
            }
        }

        private async Task Logout()
        {
            await AuthService.LogoutAsync();
            NavigationManager.NavigateTo(RoutePath.LOGIN);
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
