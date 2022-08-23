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
using System.Text.Json.Nodes;
using AgileConfig.BlazorUI.Pages;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigComparerParameter
    {
        private string[] _otherENVArray;
        private string[] _allEnv;

        public string AppId { get; set; }
        public string LeftENV { get; set; }
        public string RightENV { get; set; }
        public string[] OtherENVArray
        {
            get
            {
                _otherENVArray ??= Array.Empty<string>();
                return _otherENVArray;
            }
            set => _otherENVArray = value;
        }
        public string[] AllEnv
        {
            get
            {
                _allEnv ??= Array.Empty<string>();
                return _allEnv;
            }
            set => _allEnv = value;
        }
    }
    public partial class ConfigComparer
    {
        private string _leftString;
        private Dictionary<string, string> _leftData;
        private string _rightString;
        private bool _showSource;
        private bool _comparerAsJson = true;
        private Dictionary<string, string> _rightData;
        private string _change;
        private string _add;
        private string _sub;
        private string _diff;
        private ConfigComparerParameter _para;

        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Parameter]
        public ConfigComparerParameter Para
        {
            get
            {
                _para ??= new();
                return _para;
            }
            set => _para = value;
        }
        public bool Visible { get; set; }
        private void Cancel()
        {
            _change = string.Empty;

            _leftData = new();
            _leftString = string.Empty;
            _rightData = new();
            _rightString = string.Empty;
            _showSource = false;
            _comparerAsJson = false;
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

        private async Task LoadSourceAsync(bool value)
        {
            _showSource = value;
            if (!_showSource)
            {
                return;
            }
            _leftString = await ConfigApi.ExportJsonAsync(Para.AppId, Para.LeftENV);
            _rightString = await ConfigApi.ExportJsonAsync(Para.AppId, Para.RightENV);
        }
        private async Task LoadLeftDataAsync()
        {
            await LoadSourceAsync(_showSource);
            var leftKvRes = await ConfigApi.GetKvListAsync(Para.AppId, Para.LeftENV);
            _leftData = leftKvRes.Data.ToDictionary(s => s.Key, s => s.Value);
        }
        private async Task LoadRightDataAsync()
        {
            await LoadSourceAsync(_showSource);
            var rightKvRes = await ConfigApi.GetKvListAsync(Para.AppId, Para.RightENV);
            _rightData = rightKvRes.Data.ToDictionary(s => s.Key, s => s.Value);
        }
        private async Task ChangeLeftEnvAsync(string env)
        {
            Para.LeftENV = env;
            Para.OtherENVArray = Para.AllEnv.Where(e => e != env).ToArray();
            await LoadLeftDataAsync();
            Comparer();
            StateHasChanged();
        }
        private async Task ChangeRightEnvAsync(string env)
        {
            Para.RightENV = env;
            await LoadRightDataAsync();
            Comparer();
            StateHasChanged();
        }

        private  void ChangeComparerMode(bool type)
        {
            _comparerAsJson = type;
            Comparer();
            StateHasChanged();
        }

        private void Comparer()
        {
            _add= string.Empty;
            _sub= string.Empty;
            _diff= string.Empty;
            var addKeys = _leftData.Keys.Except(_rightData.Keys);
            var subKeys = _rightData.Keys.Except(_leftData.Keys);
            var intersectKeys = _leftData.Keys.Intersect(_rightData.Keys);
            List<string> diffKeys = new();
            foreach (string key in intersectKeys)
            {
                string left = _leftData[key];
                string right = _rightData[key];
                if (left != right)
                {
                    diffKeys.Add(key);
                }
            }
            var add = new SortedDictionary<string, string>(_leftData.Where(kv => addKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value));
            var sub = new SortedDictionary<string, string>(_rightData.Where(kv => subKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value));
            var diff = new SortedDictionary<string, string>(_leftData.Where(kv => diffKeys.Contains(kv.Key)).ToDictionary(kv => kv.Key, kv => kv.Value));
            StringBuilder stringBuilder = new();// new($"相比{_rightENV},产生了这些变更。");
            if (add.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("增加了这些节点");
                _add = GetComparerString(add);
                stringBuilder.AppendLine(_add);
            }
            if (sub.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("删除了这些节点");
                _sub = GetComparerString(sub);
                stringBuilder.AppendLine(_sub);
            }
            if (diff.Count > 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("修改了这些节点");
                _diff = GetComparerString(diff);
                stringBuilder.AppendLine(_diff);
            }
            if (add.Count == 0 && sub.Count == 0 && diff.Count == 0)
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine("无");
            }
            _change = stringBuilder.ToString();
        }
        private string GetComparerString(SortedDictionary<string, string> pairs)
        {
            return _comparerAsJson
                 ? ParseToJson(pairs)
                 : string.Join(Environment.NewLine, pairs.Select(kv => $"{kv.Key}={kv.Value}"));
        }
        private static string ParseToJson(SortedDictionary<string, string> pairs)
        {
            var root = new JsonObject();
            foreach (var item in pairs)
            {
                SetKVIntoJson(root, item);
            }
            var newRoot = ProcessJsonArray(root);
            return newRoot.ToJsonString(Json.SystemTextJsonSerializerOptions);
        }

        private static void SetKVIntoJson(JsonObject root, KeyValuePair<string, string> pair, string keyDelimiter = ":")
        {
            string[] keyLevels = pair.Key.Split(keyDelimiter, StringSplitOptions.RemoveEmptyEntries);
            JsonNode currentNode = root;
            for (int i = 0; i < keyLevels.Length; i++)
            {
                string currentKey = keyLevels[i];
                bool isPropKey = i == keyLevels.Length - 1;
                if (isPropKey)
                {
                    var jObj = currentNode.AsObject();
                    jObj[currentKey] = pair.Value;
                }
                else
                {
                    var jObj = currentNode.AsObject();
                    if (jObj.ContainsKey(currentKey))
                    {
                        currentNode = jObj[currentKey];
                    }
                    else
                    {

                        var node = new JsonObject();
                        jObj[currentKey] = node;
                        currentNode = node;
                    }
                }
            }
        }

        private static JsonNode ProcessJsonArray(JsonObject current)
        {
            var indexKeys = new SortedSet<int>();
            var valueNodes = new List<JsonNode>();
            Dictionary<string, JsonNode> newNodeDic = new Dictionary<string, JsonNode>();
            bool isJsonArray = true;
            foreach (var item in current)
            {
                string key = item.Key;
                if (int.TryParse(key, out int indexKey))
                {
                    indexKeys.Add(indexKey);
                }
                else
                {
                    isJsonArray = false;
                }
                var node = item.Value;
                if (node is JsonObject jObj)
                {
                    isJsonArray = false;
                    newNodeDic.Add(key, ProcessJsonArray(jObj));
                }
                if (node is JsonValue jValue)
                {
                    valueNodes.Add(JsonValue.Create(jValue.ToString()));
                }
            }
            foreach (var item in newNodeDic)
            {
                string key = item.Key;
                var node = item.Value;
                current[key] = node;
            }
            if (!isJsonArray)
            {
                return current;
            }
            int i = 0;
            foreach (int item in indexKeys)
            {
                if (item != i)
                {
                    return current;
                }
                i++;
            }
            return new JsonArray(valueNodes.ToArray());
        }
    }
}
