using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        private bool _dataLoading;
        private async Task ReLoadAsync()
        {
            _dataLoading = true;
            await LoadData();
            _dataLoading = false;
            StateHasChanged();
        }

        private async Task LoadData()
        {
            UIApiClient.HttpResults.ServerNodeResult res = await ServerNodeApi.AllAsync();
            this.DataSource = res?.Data ?? Array.Empty<ServerNodeVM>();
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

        private void DeleteConfirm(ServerNodeVM node)
        {
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
