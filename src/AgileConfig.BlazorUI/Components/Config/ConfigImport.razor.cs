using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Auth;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace AgileConfig.BlazorUI.Components.Config
{
    public partial class ConfigImport
    {
        Dictionary<string, ConfigVM> _dataSourceDic = new();

        [Inject]
        public AuthService AuthService { get; set; }

        [Inject]
        public IConfigApi ConfigApi { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        [Inject]
        public MessageService MessageService { get; set; }

        [Parameter]
        public EventCallback OnCompleted { get; set; }

        [Parameter]
        public ConfigImportParameter Para { get; set; } = new();
        public Dictionary<string, string> UploadHeaders { get; set; } = new();
        public string UploadUrl => Configuration["AgileConfigServer"] + "Config/PreViewJsonFile";
        public bool Visible { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _ = SetHeadersAsync();
        }

        private void Cancel()
        {
            Para = new();
            _dataSourceDic.Clear();
            Visible = false;

        }

        private void DeleteItem(string key) => _dataSourceDic.Remove(key);

        private async Task OnOkAsync()
        {
            var config = new MessageConfig()
            {
                Content = "导入中...",
                Key = $"{nameof(ConfigImport)}-{Para.AppId}"
            };
            _ = MessageService.Loading(config);
            var res = await ConfigApi.AddRangeAsync(Para.ENV, _dataSourceDic.Values);
            if (res.Success)
            {
                config.Content = "导入成功";
                await MessageService.Success(config);
                Visible = false;
            }
            else
            {
                config.Content = $"导入失败,{res.Message}";
                await MessageService.Error(config);
            }
            _= OnCompleted.InvokeAsync();

        }

        private async Task OnSingleCompleted(UploadInfo info)
        {

            var file = info.File;
            if (file.State == UploadState.Fail)
            {
                await MessageService.Error($"{file.FileName}上传失败.");
                return;
            }
            if (info.File.State == UploadState.Uploading)
            {
                Console.WriteLine(info.File);
                Console.WriteLine(info.FileList);
                return;
            }
            //var s = JsonSerializer.Deserialize<ApiResult<List<ConfigVM>>>(file.Response);
            var res = file.GetResponse<ApiResult<List<ConfigVM>>>(Consts.Json.SystemTextJsonDeserializeOptions);
            var itemList = res.Data;
            itemList.ForEach(x =>
            {
                if (_dataSourceDic.TryGetValue(x.Key, out ConfigVM vm))
                {
                    _dataSourceDic.Remove(x.Key);
                }
                _dataSourceDic.Add(x.Key, x);
            });

        }

        private async Task SetHeadersAsync()
        {
            UIApiClient.HttpResults.TokenResult authInfo = await AuthService.GetAuthInfo();
            string auth = new AuthenticationHeaderValue(authInfo.Type, authInfo.Token).ToString();
            UploadHeaders.TryAdd("Authorization", auth);
            StateHasChanged();
        }
    }

    public class ConfigImportParameter
    {
        public string AppId { get; set; }

        public string ENV { get; set; }
    }
}
