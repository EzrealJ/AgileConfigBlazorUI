using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Components.App;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpResults;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class App
    {
        #region 搜索条件
        class FormClass
        {
            public string Group { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Order { get; set; }
            public string SortField { get; set; }
            public bool TableGrouped { get; set; }
        }
        #endregion
        protected PageResult<AppListVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        AuthApp _authApp;
        AppVM _authObj = new AppVM();
        EditApp _editApp;
        AppListVM _editObj;
        EnumEditType _enumEditType;
        FormClass _formClass = new();
        InheritancedAppView _inheritancedAppView;
        AppVM _inheritancedAppViewObj = new AppVM();

        private bool _loading = false;
        IEnumerable<string> _options = new List<string>();
        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        private IAppApi AppApi { get; set; }
        [Inject]
        private ModalService ModalService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = LoadDataAsync();
        }

        private async Task AddAsync()
        {
            _enumEditType = EnumEditType.Add;
            _editObj = new();
            _editApp.Visible = true;
            await Task.CompletedTask;
        }

        private async Task AuthAsync(AppVM app)
        {
            _authObj = app;
            _authApp.Visible = true;
            await Task.CompletedTask;
        }

        private void ConfigList(AppVM app)
        {
            NavigationManager.NavigateTo($"/Config/{app.Id}");
        }

        private async Task DeleteAsync(AppListVM app)
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteAsync)}-{app.Id}"
            };
            _ = MessageService.Loading(config);
            var res = await AppApi.DeleteAsync(app.Id);
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

        private void DeleteConfirm(AppListVM app)
        {
            var options = new ConfirmOptions()
            {
                Title = $"是否确定删除节点【{app.Name}】",
                Icon = infoIcon,
                OnOk = async e => await DeleteAsync(app)
            };
            ModalService.Confirm(options);
        }

        private async Task EditAsync(AppListVM app)
        {
            _enumEditType = EnumEditType.Edit;
            _editObj = app;
            _editApp.Visible = true;
            await Task.CompletedTask;
        }

        private async Task EnabledOrDisableAsync(string id)
        {
            await AppApi.DisableOrEanbleAsync(id);
            await SearchAsync();
        }

        private async Task HandleTableChange(QueryModel<AppListVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            ITableSortModel tableSortModel = queryModel.SortModel.Last(s => !string.IsNullOrWhiteSpace(s.Sort));
            _formClass.SortField = tableSortModel.FieldName;
            _formClass.Order = tableSortModel.Sort;
            await SearchAsync();
        }

        private async Task LoadDataAsync()
        {
            var res = await AppApi.GetAppGroupsAsync();
            _options = res.Data ?? Array.Empty<string>();
            StateHasChanged();
        }

        private void ReSet() => _formClass = new();

        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(_formClass.SortField))
            {
                _formClass.SortField = nameof(AppListVM.Id);
            }
            if (string.IsNullOrWhiteSpace(_formClass.Order))
            {
                _formClass.Order = SortDirection.Ascending.Name;
            }
            _formClass.SortField = _formClass.SortField[0].ToString().ToLower() + _formClass.SortField[1..];
            _dataSource = await AppApi.SearchAsync(
                 _formClass.Name,
                 _formClass.Id,
                 _formClass.Group,
                 _formClass.SortField,
                 _formClass.Order,
                 _formClass.TableGrouped,
                 _dataSource.Current,
                 _dataSource.PageSize
                 );

            StateHasChanged();
        }
        private async Task ViewInheritancedAppAsync(AppListVM app)
        {
            _inheritancedAppViewObj = app;
            _inheritancedAppView.Visible = true;
            await Task.CompletedTask;
        }
    }
}

