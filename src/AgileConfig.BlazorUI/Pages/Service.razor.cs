using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Components.Service;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpModels;
using AgileConfig.UIApiClient.HttpResults;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Service
    {
        class FormClass
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Status { get; set; }
            public string SortField { get; set; }

            public string Order { get; set; }
        }
        [Inject]
        public IServiceApi ServiceApi { get; set; }
        [Inject]
        public ModalService ModalService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        private FormClass _formClass=new();
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
        private PageResult<ServiceInfoVM> _dataSource = new()
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
            _= SearchAsync();
        }

        private void ReSet() => _formClass = new();
        private async Task HandleTableChange(QueryModel<ServiceInfoVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            ITableSortModel tableSortModel = queryModel.SortModel.Last(s => !string.IsNullOrWhiteSpace(s.Sort));
            _formClass.SortField = tableSortModel.FieldName;
            _formClass.Order = tableSortModel.Sort;
            await SearchAsync();
        }
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
            _dataSource = await ServiceApi.SearchAsync(
                 _formClass.Name,
                 _formClass.Id,
                _formClass.Status == null ? null : Enum.Parse<ServiceStatus>(_formClass.Status),
                 _formClass.SortField,
                 _formClass.Order,
                 _dataSource.Current,
                 _dataSource.PageSize
                 );
            StateHasChanged();
        }

        private void DeleteConfirm(ServiceInfoVM service)
        {
            var options = new ConfirmOptions()
            {
                Title = $"是否确定删除服务【{service.ServiceId}】",
                Icon = infoIcon,
                OnOk = async e => await DeleteAsync(service)
            };
            ModalService.Confirm(options);
        }

        private async Task DeleteAsync(ServiceInfoVM service)
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteAsync)}-{service.Id}"
            };
            _ = MessageService.Loading(config);
            var res = await ServiceApi.RemoveAsync(service.Id);
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
            _editService.Visible=true;
            await Task.CompletedTask;
        }
    }
}
