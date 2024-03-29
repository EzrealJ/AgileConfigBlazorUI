﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpModels;
using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Client
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
        private string _address;
        private IEnumerable<string> _addresses = Array.Empty<string>();
        private bool _dataLoading;
        private PageResult<ClientVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20,
        };

        private EnumItemShowType _itemShowType;
        [Inject]
        public MessageService MessageService { get; set; }

        [Inject]
        public ModalService ModalService { get; set; }

        [Inject]
        public IRemoteServerProxyApi RemoteServerProxyApi { get; set; }

        [Inject]
        public IReportApi ReportApi { get; set; }

        [Inject]
        public IServerNodeApi ServerNodeApi { get; set; }

        private static (Dictionary<string, int> X, int Y) Gutter => (_gutterX, _gutterY);
        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = LoadDataAsync();
        }

        private void ChangeShowType()
                    => _itemShowType = _itemShowType == EnumItemShowType.TableRow ? EnumItemShowType.Card : EnumItemShowType.TableRow;
        private async Task HandleTableChange(QueryModel<ClientVM> queryModel)
        {
            _dataSource.Current = queryModel.PageIndex;
            _dataSource.PageSize = queryModel.PageSize;
            //ITableSortModel tableSortModel = queryModel.SortModel.Last(s => !string.IsNullOrWhiteSpace(s.Sort));
            await SearchAsync();
        }

        private async Task LoadDataAsync()
        {
            var nodes = await ServerNodeApi.AllAsync();
            _addresses = new string[] { string.Empty }.Concat(nodes?.Data?.Select(n => n.Address) ?? Array.Empty<string>());
            await SearchAsync();
        }

        private async Task OfflineAsync(ClientVM client)
        {
            var config = new MessageConfig()
            {
                Content = "断开中...",
                Key = $"{nameof(OfflineAsync)}-{client.Id}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await RemoteServerProxyApi.OfflineAsync(client.Address, client.Id);
            if (res.Success)
            {
                config.Content = "断开成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"断开失败,{res.Message}";
                await MessageService.Error(config);
            }
        }

        private void OfflineConfirm(ClientVM client)
        {
            ModalService.DestroyAllConfirmAsync();
            var options = new ConfirmOptions
            {
                Title = $"是否确定断开与客户端【{client.Id}】的连接？",
                Icon = infoIcon,
                OnOk = async e => await OfflineAsync(client)
            };
            ModalService.Confirm(options);
        }

        private async Task ReloadClientAsync(ClientVM client)
        {
            var config = new MessageConfig()
            {
                Content = "刷新中...",
                Key = $"{nameof(ReloadClientAsync)}-{client.Id}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await RemoteServerProxyApi.ReloadAsync(client.Address, client.Id);
            if (res.Success)
            {
                config.Content = "刷新成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"刷新失败,{res.Message}";
                await MessageService.Warn(config);
            }
        }

        private void ReSet()
        {
            _address = string.Empty;
            StateHasChanged();
        }
        private async Task SearchAsync()
        {
            _dataLoading = true;
            _dataSource = await ReportApi.SearchServerNodeClientsAsync(_address, _dataSource.Current, _dataSource.PageSize);
            _dataLoading = false;
            StateHasChanged();

        }
    }
}
