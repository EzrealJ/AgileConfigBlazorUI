using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Consts;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.BlazorUI.Pages;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.App
{
    public partial class AuthApp
    {
        public string Title => $"{AppName} - 用户授权";
        [Parameter]
        public string AppId { get; set; }
        [Parameter]
        public string AppName { get; set; }
        [Parameter]
        public EventCallback<MouseEventArgs> OnCompleted { get; set; }
        [Inject]
        private IAppApi AppApi { get; set; }
        [Inject]
        public IUserApi UserApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public IUserPermissionChecker UserPermissionChecker { get; set; }
        [Inject]
        public AuthService AuthService { get; set; }

        public bool Visible { get; set; }

        private AppAuthVM CurrentObject { get; set; } = new();

        public IEnumerable<string> EditConfigPermissionUsers
        {
            get => CurrentObject.EditConfigPermissionUsers;
            set => CurrentObject.EditConfigPermissionUsers = value.ToList();
        }


        public IEnumerable<string> PublishConfigPermissionUsers
        {
            get => CurrentObject.PublishConfigPermissionUsers;
            set => CurrentObject.PublishConfigPermissionUsers = value.ToList();
        }


        private bool HasAuthPermission => UserPermissionChecker.CheckUserPermission(AuthService.GetFunctions(), JudgeKey.APP_AUTH, AppId);
        private ButtonProps OkButtonProps
            => HasAuthPermission ? new ButtonProps { Disabled = true } : new ButtonProps { };
        private IEnumerable<UserVM> _userAdminOptions = new List<UserVM>();
        private Form<AppAuthVM> _form;

        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var res2 = await UserApi.AllUsersAsync();
            _userAdminOptions = res2.Data ?? Array.Empty<UserVM>();
            var res = await AppApi.GetUserAppAuthAsync(AppId);
            CurrentObject = res.Data;
        }
        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
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
