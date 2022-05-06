using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AgileConfig.BlazorUI.Model
{
    public class JsonData
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }
}
