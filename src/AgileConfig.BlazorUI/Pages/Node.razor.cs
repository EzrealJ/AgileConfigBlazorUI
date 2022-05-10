using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Components.Node;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Node
    {
        [Inject] private IServerNodeApi ServerNodeApi { get; set; }
        [Inject] private IRemoteServerProxyApi RemoteServerProxyApi { get; set; }
        [Inject] private MessageService MessageService { get; set; }
        [Inject] private ModalService ModalService { get; set; }

        public IEnumerable<ServerNodeVM> DataSource { get; set; }

     

        private EnumItemShowType _itemShowType = EnumItemShowType.TableRow;
        private string ShowTypeString => _itemShowType == EnumItemShowType.Card ? "表格显示" : "卡片显示";
        private bool _dataLoading;
        private EditNode _editNode;

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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _= LoadAllAsync();
        }
        private async Task LoadAllAsync()
        {
            _dataLoading = true;
            UIApiClient.HttpResults.ServerNodeResult res = await ServerNodeApi.AllAsync();
            this.DataSource = res?.Data ?? Array.Empty<ServerNodeVM>();
            _dataLoading = false;
            StateHasChanged();
        }   

        private async Task ReloadAllClientAsync(string address)
        {
            var config = new MessageConfig()
            {
                Content = "刷新中...",
                Key = $"{nameof(ReloadAllClientAsync)}-{address}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await RemoteServerProxyApi.ReloadAsync(address);
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

        private void ChangeShowType()
        {
            _itemShowType = _itemShowType == EnumItemShowType.TableRow
                ? EnumItemShowType.Card
                : EnumItemShowType.TableRow;
        }


        private void DeleteConfirm(ServerNodeVM node)
        {
            ModalService.DestroyAllConfirmAsync();
            var options = new ConfirmOptions
            {
                Title = $"是否确定删除节点【{node.Address}】",
                Icon = infoIcon,
                Content = "删除节点并不会让其真正的下线，只是脱离控制台的管理。所有连接至此节点的客户端都会继续正常工作。",
                OnOk = async e => await DeleteAsync(node)
            };
            ModalService.Confirm(options);
        }
        private async Task DeleteAsync(ServerNodeVM node)
        {
            var config = new MessageConfig()
            {
                Content = "删除中...",
                Key = $"{nameof(DeleteAsync)}-{node.Address}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await ServerNodeApi.DeleteAsync(node);
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
        private async Task ClientOfflineAsync(string address)
        {
            var config = new MessageConfig()
            {
                Content = "下线中...",
                Key = $"{nameof(ReloadAllClientAsync)}-{address}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await RemoteServerProxyApi.ReloadAsync(address);
            if (res.Success)
            {
                config.Content = "下线成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"下线失败,{res.Message}";
                await MessageService.Error(config);
            }
        }
    }
}
