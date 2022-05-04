using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AgileConfig.UIApiClient;
using System.Linq;
using AntDesign;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Config
{

    public class ConfigEditorParameter
    {
        public string ConfigType { get; set; } = "JSON";
        public string AppId { get; set; }
        public string ENV { get; set; }
    }
    public partial class ConfigEditor
    {
        [Inject]
        private IConfigApi ConfigApi { get; set; }

        private MessageService MessageService { get; set; }

        string _value;
        [Parameter]
        public ConfigEditorParameter ConfigEditorParameter { get; set; }

        public EventCallback<MouseEventArgs> OnCompleted { get; set; }


        public bool Visible { get; set; }

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
                await OnCompleted.InvokeAsync(e);
            }
        }

        private void Cancel()
        {
            ConfigEditorParameter = new();
            Visible = false;
        }

        public class JsonData
        {
            [JsonExtensionData]
            public Dictionary<string, JsonElement> ExtensionData { get; set; }
        }
        private string _dataError;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            AllowTrailingCommas = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
            WriteIndented = true,
        };
        private async Task CheckJsonAsync(string value)
        {
            try
            {
                var json = JsonSerializer.Deserialize<JsonData>(value, _jsonSerializerOptions);
                _value = JsonSerializer.Serialize(json, _jsonSerializerOptions);
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



    }
}
