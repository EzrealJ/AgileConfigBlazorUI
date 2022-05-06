using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using AgileConfig.BlazorUI.Auth;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigImportParameter
    {
        public string AppId { get; set; }

        public string ENV { get; set; }
    }
    public partial class ConfigImport
    {
        [Parameter]
        public ConfigImportParameter Para { get; set; } = new();
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        public bool Visible { get; set; }

        public string UploadUrl => Configuration["AgileConfigServer"] + "Config/PreViewJsonFile";

        public Dictionary<string, string> UploadHeaders { get; set; } = new();
        private async Task SetHeadersAsync()
        {
            UIApiClient.HttpResults.LoginResult authInfo = await AuthService.GetAuthInfo();
            string auth = new AuthenticationHeaderValue(authInfo.Type, authInfo.Token).ToString();
            UploadHeaders.TryAdd("Authorization", auth);
        }

        [Inject]
        public MessageService MessageService { get; set; }
        [Inject]
        public IConfiguration Configuration { get; set; }
        [Inject]
        public AuthService AuthService { get; set; }
        [Inject]
        public IConfigApi ConfigApi { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await SetHeadersAsync();
            await base.OnInitializedAsync();
        }

        private async Task OnOkAsync()
        {
            var config = new MessageConfig()
            {
                Content = "导入中...",
                Key = $"{nameof(ConfigImport)}-{Para.AppId}"
            };
            _ = MessageService.Loading(config);
            var res = await ConfigApi.AddRangeAsync(Para.ENV,_dataSourceDic.Values);
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
            await OnCompleted.InvokeAsync();

        }
        private void Cancel()
        {
            Para = new();
            _dataSourceDic.Clear();
            Visible = false;

        }
        Dictionary<string, ConfigVM> _dataSourceDic = new();
        private void DeleteItem(string key) => _dataSourceDic.Remove(key);
        private async Task OnSingleCompleted(UploadInfo info)
        {

            var file = info.File;
            if (file.State == UploadState.Fail)
            {
                await MessageService.Error($"{ file.FileName}上传失败.");
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
    }
}
