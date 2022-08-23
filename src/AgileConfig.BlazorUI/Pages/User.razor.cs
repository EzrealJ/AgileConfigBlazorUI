using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.BlazorUI.Components.User;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.BlazorUI.Extensions;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class User
    {
        private static readonly Dictionary<string, int> _gutterX = new()
        {
            ["xs"] = 8,
            ["sm"] = 16,
            ["md"] = 24,
            ["lg"] = 32,
            ["xl"] = 48,
            ["xxl"] = 64
        };

        private static readonly int _gutterY = 24;

        private PageResult<UserVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        private UserVM _editObj;

        private EditUser _editUser;

        private FormClass _formClass = new();
        private EnumEditType _editType;
        private EnumItemShowType _itemShowType;

        [Inject]
        public AuthService AuthService { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public ModalService ModalService { get; set; }

        [Inject]
        public IUserApi UserApi { get; set; }

        private static (Dictionary<string, int> X, int Y) Gutter => (_gutterX, _gutterY);

        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = SearchAsync();
        }

        private async Task AddAsync()
        {
            _editUser.Visible = true;
            _editType= EnumEditType.Add;
            _editObj = new();
            await Task.CompletedTask;
        }

        private void ChangeShowType()
       => _itemShowType = _itemShowType == EnumItemShowType.TableRow ? EnumItemShowType.Card : EnumItemShowType.TableRow;

        private bool CheckUserListModifyPermission(UserVM user)
        {
            var authMap = Enum.GetValues<EnumRole>().ToDictionary(e => e.ToString(), e => e.GetIntValue());
            var currentAuthNum = EnumRole.NormalUser;
            var roles = AuthService.GetAuthority();
            if (roles?.Count > 0)
            {
                int maxPermission = roles.Select(x => authMap[x]).Min();
                currentAuthNum = (EnumRole)maxPermission;
            }
            var userAuthNum = user.UserRoles.Min();

            return currentAuthNum < userAuthNum;
        }

        private async Task DeleteAsync(UserVM user)
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteAsync)}-{user.Id}"
            };
            _ = MessageService.Loading(config);
            var res = await UserApi.DeleteAsync(user.Id);
            if (res.Success)
            {
                config.Content = "删除成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"删除失败,{res.Message}";
                await MessageService.Error(config);
            }
        }

        private void DeleteConfirm(UserVM user)
        {
            var options = new ConfirmOptions()
            {
                Title = $"是否确定删除用户【{user.UserName}】",
                Icon = infoIcon,
                OnOk = async e => await DeleteAsync(user)
            };
            ModalService.Confirm(options);
        }

        private async Task EditAsync(UserVM user)
        {
            _editUser.Visible = true;
            _editType = EnumEditType.Edit;
            _editObj = user;
            await Task.CompletedTask;
        }

        private void ReSet() => _formClass = new();

        private async Task ResetPasswordAsync(UserVM user)
        {
            var config = new MessageConfig()
            {
                Content = "重置中...",
                Key = $"{nameof(DeleteAsync)}-{user.Id}"
            };
            _ = MessageService.Loading(config);
            var res = await UserApi.ResetPasswordAsync(user.Id);
            if (res.Success)
            {
                config.Content = "重置成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"重置失败,{res.Message}";
                await MessageService.Error(config);
            }
        }

        private void ResetPasswordConfirm(UserVM user)
        {
            var options = new ConfirmOptions()
            {
                Title = $"确定重置用户【{user.UserName}】的密码为【123456】？",
                Icon = infoIcon,
                OnOk = async e => await ResetPasswordAsync(user)
            };
            ModalService.Confirm(options);
        }

        private async Task SearchAsync()
        {
            _dataSource = await UserApi.SearchAsync(
                 _formClass.UserName,
                 _formClass.Team,
                 _dataSource.Current,
                 _dataSource.PageSize
                 );
            StateHasChanged();
        }

        class FormClass
        {
            public string Team { get; set; }
            public string UserName { get; set; }
        }
    }
}
