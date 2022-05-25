using AgileConfig.BlazorUI.Components.App;
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
using AgileConfig.BlazorUI.Components.Config;
using Blazored.LocalStorage;
using AgileConfig.BlazorUI.Extensions;
using static AgileConfig.BlazorUI.Components.Config.ConfigEnvironmentSync;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Config
    {
        #region 搜索条件
        class FormClass
        {

            public string Group { get; set; }
            public string Key { get; set; }
            public string ENV { get; set; }
            public string SortField { get; set; }
            public string Order { get; set; }

        }
        #endregion
        string _onlineStatusStr;
        FormClass _formClass = new();
        EditConfig _editConfig;
        ConfigEditor _configEditor;
        EnumEditType _enumEditType;
        ConfigVM _editObj;
        IEnumerable<ConfigVM> _selectedRows;
        private bool _loading = false;
        private readonly IEnumerable<string> _options = Enum.GetValues<OnlineStatus>().Select(e => e.GetDescription());
        string[] _envs = Array.Empty<string>();
        private ConfigItemHistory _configItemHistory;
        private ConfigItemHistoryPara _configItemHistoryPara;
        private ConfigHistory _configHistory;
        private ConfigHistoryParameter _configHistoryPara;
        private ConfigEnvironmentSync _configEnvironmentSync;
        private ConfigEnvSyncParameter _configEnvSyncParameter;
        private ConfigImport _configImport;
        private ConfigImportParameter _configImportParameter;
        private ConfigExport _configExport;
        private ConfigExportParameter _configExportParameter;

        protected PageResult<ConfigVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        [Parameter]
        public string AppId { get; set; }
        [Inject]
        private IConfigApi ConfigApi { get; set; }
        [Inject]
        private IHomeApi HomeApi { get; set; }
        [Inject]
        private ModalService ModalService { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = LoadDataAsync();
        }
        
        private async Task LoadDataAsync()
        {
            var res = await HomeApi.SysAsync();
            _envs = res.EnvList;
            _formClass.ENV = _envs.FirstOrDefault();
            StateHasChanged();
        }

        private void ReSet() => _formClass = new() { ENV = _envs.FirstOrDefault() };
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
            _dataSource = await ConfigApi.SearchAsync(
                 AppId,
                 _formClass.Group,
                 _formClass.Key,
                _onlineStatusStr == null ? null : Enum.Parse<OnlineStatus>(_onlineStatusStr),
                 _formClass.SortField,
                 _formClass.Order,
                 _formClass.ENV,
                 _dataSource.PageSize,
                 _dataSource.Current
                 );

            StateHasChanged();
        }


        private async Task HandleTableChange(QueryModel<ConfigVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            ITableSortModel tableSortModel = queryModel.SortModel.Last(s => !string.IsNullOrWhiteSpace(s.Sort));
            _formClass.SortField = tableSortModel.FieldName;
            _formClass.Order = tableSortModel.Sort;
            await SearchAsync();
        }

        private void DeleteConfirm(ConfigVM app)
        {
            var options = new ConfirmOptions()
            {
                Title = $"是否确定删除配置【{app.Key}】",
                Icon = infoIcon,
                OnOk = async e => await DeleteAsync(app)
            };
            ModalService.Confirm(options);
        }

        private async Task DeleteAsync(ConfigVM app)
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteAsync)}-{app.Id}"
            };
            _ = MessageService.Loading(config);
            var res = await ConfigApi.DeleteAsync(app.Id, _formClass.ENV);
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

        ConfigEditorParameter _configEditorParameter = new();
        private async Task EditAsJsonAsync()
        {
            _configEditorParameter = new()
            {
                ConfigType = "JSON",
                AppId = this.AppId,
                ENV = _formClass.ENV,
            };
            _configEditor.Visible = true;
            StateHasChanged();
            await Task.CompletedTask;


        }
        private async Task EditAsTextAsync()
        {

            _configEditorParameter = new()
            {
                ConfigType = "TEXT",
                AppId = this.AppId,
                ENV = _formClass.ENV,
            };
            _configEditor.Visible = true;
            StateHasChanged();
            await Task.CompletedTask;

        }
        private void CancelSelectConfirm()
        {
            var options = new ConfirmOptions()
            {
                Title = $"确定撤销选中配置项的编辑状态吗？?",
                Icon = infoIcon,
                OnOk = async e => await CancelSelectAsync()
            };
            ModalService.Confirm(options);
        }
        private async Task CancelSelectAsync()
        {
            var config = new MessageConfig()
            {
                Content = "撤销中...",
                Key = $"{nameof(CancelSelectAsync)}-{AppId}"
            };
            var items = _selectedRows?.Where(c => c.EditStatus != EditStatus.Commit).Select(c => c.Id);
            var res = await ConfigApi.CancelSomeEditAsync(_formClass.ENV, items);
            if (res.Success)
            {
                config.Content = "撤销成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"撤销失败,{res.Message}";
                await MessageService.Error(config);
            }
        }
        private void DeleteSelectConfirm()
        {
            var options = new ConfirmOptions()
            {
                Title = $"确定删除选中的配置项吗?",
                Icon = infoIcon,
                OnOk = async e => await DeleteSelectAsync()
            };
            ModalService.Confirm(options);
        }
        private async Task DeleteSelectAsync()
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteSelectAsync)}-{AppId}"
            };
            var items = _selectedRows?.Select(c => c.Id);
            var res = await ConfigApi.DeleteSomeAsync(_formClass.ENV, items);
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
            _editConfig.Visible = true;
            await Task.CompletedTask;
        }
        private async Task EditAsync(ConfigVM app)
        {
            //await MessageService.Info("点击了编辑");
            _enumEditType = EnumEditType.Edit;
            _editObj = app;
            _editConfig.Visible = true;
            await Task.CompletedTask;
        }
        private void ItemCancelConfirm(ConfigVM app)
        {
            var options = new ConfirmOptions()
            {
                Title = $"确定撤销对配置【{app.Key}:{app.Value}】的改动吗?",
                Icon = infoIcon,
                OnOk = async e => await ItemCancelAsync(app)
            };
            ModalService.Confirm(options);
        }
        private async Task ItemCancelAsync(ConfigVM app)
        {
            _configItemHistory.Visible = true;
            _configItemHistoryPara = new ConfigItemHistoryPara
            {
                Config = app,
                ENV = _formClass.ENV
            };
            await Task.CompletedTask;
        }
        private async Task ItemHistoryAsync(ConfigVM app)
        {
            _configItemHistory.Visible = true;
            _configItemHistoryPara = new ConfigItemHistoryPara
            {
                Config = app,
                ENV = _formClass.ENV
            };
            await Task.CompletedTask;
        }
        private async Task HistoryAsync()
        {
            _configHistory.Visible = true;
            _configHistoryPara = new ConfigHistoryParameter
            {
                AppId = AppId,
                ENV = _formClass.ENV
            };
            await Task.CompletedTask;
        }
        private async Task EnvironmentSyncAsync()
        {
            _configEnvironmentSync.Visible = true;
            _configEnvSyncParameter = new ConfigEnvSyncParameter
            {
                AppId = AppId,
                CurrentEnvironment = _formClass.ENV,
                SyncableEnvironments = _envs.Where(e => e != _formClass.ENV).ToArray()
            };
            await Task.CompletedTask;
        }

        private async Task JsonImportAsync()
        {
            _configImport.Visible = true;
            _configImportParameter = new ConfigImportParameter
            {
                AppId = AppId,
                ENV = _formClass.ENV,
            };
            await Task.CompletedTask;
        }

        private async Task JsonExportAsync()
        {
            _configExport.Visible = true;
            _configExportParameter = new ConfigExportParameter
            {
                AppId = AppId,
                ENV = _formClass.ENV,
            };
            await Task.CompletedTask;
        }
    }
}
