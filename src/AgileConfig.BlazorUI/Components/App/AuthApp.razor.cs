using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.App
{
    public partial class AuthApp
    {
        private Form<AppAuthVM> _form;
        private IEnumerable<UserVM> _userAdminOptions = new List<UserVM>();
        [Parameter]
        public string AppId { get; set; }

        [Parameter]
        public string AppName { get; set; }

        [Inject]
        public AuthService AuthService { get; set; }

        public IEnumerable<string> EditConfigPermissionUsers
        {
            get => CurrentObject.EditConfigPermissionUsers;
            set => CurrentObject.EditConfigPermissionUsers = value.ToList();
        }

        [Inject]
        public MessageService MessageService { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnCompleted { get; set; }

        public IEnumerable<string> PublishConfigPermissionUsers
        {
            get => CurrentObject.PublishConfigPermissionUsers;
            set => CurrentObject.PublishConfigPermissionUsers = value.ToList();
        }

        public string Title => $"{AppName} - 用户授权";
        [Inject]
        public IUserApi UserApi { get; set; }

        [Inject]
        public IUserPermissionChecker UserPermissionChecker { get; set; }

        public bool Visible { get; set; }

        [Inject]
        private IAppApi AppApi { get; set; }
        private AppAuthVM CurrentObject { get; set; } = new();
        private bool HasAuthPermission => UserPermissionChecker.CheckUserPermission(AuthService.GetFunctions(), JudgeKey.APP_AUTH, AppId);
        private ButtonProps OkButtonProps
            => HasAuthPermission ? new ButtonProps { Disabled = true } : new ButtonProps { };
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            await LoadDataAsync();
        }

        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
        }

        private async Task LoadDataAsync()
        {
            var res2 = await UserApi.AllUsersAsync();
            _userAdminOptions = res2.Data ?? Array.Empty<UserVM>();
            var res = await AppApi.GetUserAppAuthAsync(AppId);
            CurrentObject = res.Data;
        }
        private async Task OnOkAsync(MouseEventArgs e)
        {
            var config = new MessageConfig()
            {
                Content = $"正在授权...",
                Key = $"{nameof(AuthApp)}-{CurrentObject.AppId}"
            };
            var res = await AppApi.SaveAppAuthAsync(CurrentObject);
            if (res.Success)
            {
                config.Content = $"授权成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"授权失败,{res.Message}";
                await MessageService.Error(config);
            }
            Visible = false;
            if (OnCompleted.HasDelegate)
            {
                await OnCompleted.InvokeAsync(e);
            }
        }
    }
}
