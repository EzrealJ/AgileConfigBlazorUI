using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient.HttpResults;
using AgileConfig.UIApiClient;
using AntDesign.TableModels;
using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AgileConfig.BlazorUI.Components.User;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class User
    {
        class FormClass
        {
            public string UserName { get; set; }
            public string Team { get; set; }
        }
        [Inject]
        public IUserApi UserApi { get; set; }
        [Inject]
        public ModalService ModalService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        private FormClass _formClass = new();
        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";
        private static (Dictionary<string, int> X, int Y) Gutter => (_gutterX, _gutterY);
        private static readonly int _gutterY = 24;
        private static readonly Dictionary<string, int> _gutterX = new()
        {
            ["xs"] = 8,
            ["sm"] = 16,
            ["md"] = 24,
            ["lg"] = 32,
            ["xl"] = 48,
            ["xxl"] = 64
        };
        private PageResult<UserVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        private EnumItemShowType _itemShowType;
        private void ChangeShowType()
       => _itemShowType = _itemShowType == EnumItemShowType.TableRow ? EnumItemShowType.Card : EnumItemShowType.TableRow;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = SearchAsync();
        }

        private void ReSet() => _formClass = new();

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
        private EditUser _editUser;
        private UserVM _editObj;
        private async Task AddAsync()
        {
            _editUser.Visible = true;
            _editUser.EditType = EnumEditType.Add;
            _editObj = new();
            await Task.CompletedTask;
        }
        private async Task EditAsync(UserVM user)
        {
            _editUser.Visible = true;
            _editUser.EditType = EnumEditType.Edit;
            _editObj = user;
            await Task.CompletedTask;
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
    }
}
