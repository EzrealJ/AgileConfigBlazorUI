using System.Text.Json;
using System;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Model;
using AgileConfig.UIApiClient;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using AgileConfig.BlazorUI.Helpers;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using AgileConfig.BlazorUI.Consts;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigComparerParameter
    {
        public string AppId { get; set; }
        public string LeftENV { get; set; }
        public string[] OtherENVArray { get; set; }
    }
    public partial class ConfigComparer
    {
        private string _leftString;
        private JsonData _leftData;
        private string _rightString;
        private string _rightENV;
        private JsonData _rightData;
        private string _change;

        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Parameter]
        public ConfigComparerParameter Para { get; set; }
        public bool Visible { get; set; }
        private void Cancel()
        {
            _change = string.Empty;

            _leftData = new();
            _leftString = string.Empty;


            _rightData = new();
            _rightENV = string.Empty;
            _rightString = string.Empty;
            Para = new();
            Visible = false;
        }
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            await LoadLeftDataAsync();
        }
        private async Task LoadLeftDataAsync()
        {
            _leftString = await ConfigApi.ExportJsonAsync(Para.AppId, Para.LeftENV);
            _leftData = JsonDataHelper.SerializeJson(ref _leftString);
        }
        private async Task LoadRightDataAsync(string env)
        {
            _change=String.Empty;
            StateHasChanged();
            _rightString = await ConfigApi.ExportJsonAsync(Para.AppId, env);
            _rightData = JsonDataHelper.SerializeJson(ref _rightString);
            var addKeys = _leftData.ExtensionData.Keys.Except(_rightData.ExtensionData.Keys);
            var subKeys = _rightData.ExtensionData.Keys.Except(_leftData.ExtensionData.Keys);
            var intersectKeys = _leftData.ExtensionData.Keys.Intersect(_rightData.ExtensionData.Keys);
            List<string> diffKeys = new();
            foreach (var key in intersectKeys)
            {
                var left = _leftData.ExtensionData[key];
                var right = _rightData.ExtensionData[key];
                string leftStr = left.GetRawText();
                string rightStr = right.GetRawText();
                if (!left.ValueEquals(rightStr))
                {
                    diffKeys.Add(key);
                }
            }
            var add = _leftData.ExtensionData.Where(kv => addKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
            var sub = _rightData.ExtensionData.Where(kv => subKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
            var diff = _leftData.ExtensionData.Where(kv => diffKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value);
            StringBuilder stringBuilder = new($"相比{_rightENV},产生了这些变更。");
            if (add.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("增加了这些节点");
                stringBuilder.AppendLine(JsonSerializer.Serialize(add, Consts.Json.SystemTextJsonSerializerOptions));
            }
            if (sub.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("删除了这些节点");
                stringBuilder.AppendLine(JsonSerializer.Serialize(sub, Consts.Json.SystemTextJsonSerializerOptions));
            }
            if (diff.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("修改了这些节点");
                stringBuilder.AppendLine(JsonSerializer.Serialize(diff, Consts.Json.SystemTextJsonSerializerOptions));
            }
            if (add.Count == 0 && sub.Count == 0 && diff.Count == 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("无");
            }
            _change = stringBuilder.ToString();
        }

    }
}
