using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        FormClass _formClass = new();

        bool _loading = false;

        protected PageResult<AppListVM> _dataSource  = new()
        {
            Current = 1,
            PageSize = 20
        };
        #endregion
        [Inject]
        private IAppApi AppApi { get; set; }

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
        private async Task HandleTableChange(QueryModel<AppListVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            foreach (var item in queryModel.SortModel)
            {

                Console.WriteLine($"{item.FieldName},{item.Sort}");
            }
            ITableSortModel tableSortModel = queryModel.SortModel.Last(s=>!string.IsNullOrWhiteSpace(s.Sort));
            _formClass.SortField = tableSortModel.FieldName;
            _formClass.Order = tableSortModel.Sort;
            await SearchAsync();
        }
    }
}

