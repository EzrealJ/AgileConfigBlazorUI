using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AgileConfig.BlazorUI.Model
{
    public class JsonData
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }
}
