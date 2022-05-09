using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpModels;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Client
    {
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
        private EnumItemShowType _itemShowType;
        private bool _dataLoading;
        private string _address;
        private IEnumerable<string> _addresses = Array.Empty<string>();
        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";

        private PageResult<ClientVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20,
        };
        [Inject]
        public IReportApi ReportApi { get; set; }
        [Inject]
        public IServerNodeApi ServerNodeApi { get; set; }
        [Inject]
        public IRemoteServerProxyApi RemoteServerProxyApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public ModalService ModalService { get; set; }

        private void ChangeShowType()
            => _itemShowType = _itemShowType == EnumItemShowType.TableRow ? EnumItemShowType.Card : EnumItemShowType.TableRow;
        private void ReSet()
        {
            _address = string.Empty;
            StateHasChanged();
        }

        private async Task ReLoadAsync()
        {
            _dataLoading = true;
            await LoadData();
            _dataLoading = false;
        }
        protected override async Task OnInitializedAsync()
        {
            var nodes = await ServerNodeApi.AllAsync();
            _addresses = new string[] { string.Empty }.Concat(nodes?.Data.Select(n => n.Address) ?? Array.Empty<string>());
            await LoadData();
        }
        private async Task LoadData()
        {
            _dataSource = await ReportApi.SearchServerNodeClientsAsync(_address, _dataSource.Current, _dataSource.PageSize);
            StateHasChanged();
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
    }
}
