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
            public string Id { get; set; }
            public string Name { get; set; }
            public string Group { get; set; }
            public string SortField { get; set; }

            public string Order { get; set; }

            public bool TableGrouped { get; set; }
        }
        #endregion
        FormClass _formClass = new();
        EditApp _editApp;
        AuthApp _authApp;
        EnumEditType _enumEditType;
        AppListVM _editObj;
        AppVM _authObj=new AppVM();

        private bool _loading = false;
        IEnumerable<string> _options = new List<string>();

        protected PageResult<AppListVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        [Inject]
        private IAppApi AppApi { get; set; }
        [Inject]
        private ModalService ModalService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var res = await AppApi.GetAppGroupsAsync();
            _options = res.Data ?? Array.Empty<string>();
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

        private async Task EnabledOrDisableAsync(string id)
        {
            await AppApi.DisableOrEanbleAsync(id);
            await SearchAsync();
        }
        private async Task HandleTableChange(QueryModel<AppListVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            foreach (var item in queryModel.SortModel)
            {
                Console.WriteLine($"{item.FieldName},{item.Sort}");
            }
            ITableSortModel tableSortModel = queryModel.SortModel.Last(s => !string.IsNullOrWhiteSpace(s.Sort));
            _formClass.SortField = tableSortModel.FieldName;
            _formClass.Order = tableSortModel.Sort;
            await SearchAsync();
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

        private async Task AddAsync()
        {
            //await MessageService.Info("点击了新建");
            _enumEditType = EnumEditType.Add;
            _editObj = new();
            _editApp.Visible = true;
            await Task.CompletedTask;
        }
        private async Task EditAsync(AppListVM app)
        {
            //await MessageService.Info("点击了编辑");
            _enumEditType = EnumEditType.Edit;
            _editObj = app;
            _editApp.Visible = true;
            await Task.CompletedTask;
        }
        private async Task ConfigListAsync()
        {
            await MessageService.Info("点击了配置项列表");
        }
        private async Task AuthAsync(AppVM app)
        {
            _authObj = app;
            _authApp.Visible = true;
            await Task.CompletedTask;
        }
    }
}

