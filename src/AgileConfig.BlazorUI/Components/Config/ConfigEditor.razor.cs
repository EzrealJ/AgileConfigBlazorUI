using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Model;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Config
{

    public partial class ConfigEditor
    {
        private string _dataError;

        string _value;

        [Parameter]
        public ConfigEditorParameter ConfigEditorParameter { get; set; }

        public EventCallback<MouseEventArgs> OnCompleted { get; set; }

        public bool Visible { get; set; }

        [Inject]
        private IConfigApi ConfigApi { get; set; }

        private MessageService MessageService { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            if (ConfigEditorParameter.ConfigType == "JSON")
            {
                ApiResult<string> res = await ConfigApi.GetJsonAsync(ConfigEditorParameter.AppId, ConfigEditorParameter.ENV);
                _value = res.Data;
            }
            if (ConfigEditorParameter.ConfigType == "TEXT")
            {
                var res = await ConfigApi.GetKvListAsync(ConfigEditorParameter.AppId, ConfigEditorParameter.ENV);
                _value = string.Join(Environment.NewLine, res?.Data?.Select(kv => $"{kv.Key}={kv.Value}"));
            }

        }

        private void Cancel()
        {
            ConfigEditorParameter = new();
            Visible = false;
        }

        private async Task CheckJsonAsync(string value)
        {
            try
            {
                var json = JsonSerializer.Deserialize<JsonData>(value, Consts.Json.SystemTextJsonDeserializeOptions);
                _value = JsonSerializer.Serialize(json, Consts.Json.SystemTextJsonSerializerOptions);
                _dataError = string.Empty;
            }
            catch (Exception ex)
            {
                _dataError = "Json格式错误";
            }
            await Task.CompletedTask;
        }

        private async Task CheckTextAsync(string value)
        {
            try
            {
                string[] texts = value.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                int i = 0;
                foreach (string v in texts)
                {
                    i++;
                    string temp = v.TrimStart().TrimEnd();
                    if (!temp.Contains('='))
                    {
                        _dataError = $"第{i}行,缺少=";
                        return;
                    }
                    if (temp.IndexOf('=') == 0)
                    {
                        _dataError = $"第{i}行,缺少KEY";
                        return;
                    }
                    if (temp.IndexOf('=') == temp.Length - 1)
                    {
                        _dataError = $"第{i}行,缺少Value";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _dataError = "TEXT格式错误";
            }
            await Task.CompletedTask;
        }

        private async Task OnOkAsync(MouseEventArgs e)
        {
            var config = new MessageConfig()
            {
                Content = $"正在保存...",
                Key = $"{ConfigEditorParameter.AppId}-{ConfigEditorParameter.ENV}"
            };
            ApiResult res = new();
            if (ConfigEditorParameter.ConfigType == "JSON")
            {
                res = await ConfigApi.SaveJsonAsync(ConfigEditorParameter.AppId, ConfigEditorParameter.ENV, new SaveJsonVM
                {
                    Json = _value
                });
            }
            if (ConfigEditorParameter.ConfigType == "TEXT")
            {
                res = await ConfigApi.SaveKvListAsync(ConfigEditorParameter.AppId, ConfigEditorParameter.ENV, new SaveKVListVM
                {
                    Str = _value
                });
            }
            if (res.Success)
            {
                config.Content = $"保存成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"保存失败,{res.Message}";
                await MessageService.Error(config);
            }
            Visible = false;
            if (OnCompleted.HasDelegate)
            {
                _ = OnCompleted.InvokeAsync(e);
            }
        }
    }

    public class ConfigEditorParameter
    {
        public string AppId { get; set; }
        public string ConfigType { get; set; } = "JSON";
        public string ENV { get; set; }
    }
}
