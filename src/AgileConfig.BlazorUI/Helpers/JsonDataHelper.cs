using AgileConfig.BlazorUI.Model;
using System.Text.Json;
using System;

namespace AgileConfig.BlazorUI.Helpers
{
    public class JsonDataHelper
    {
        public static JsonData SerializeJson(ref string value)
        {
            try
            {
                var json = JsonSerializer.Deserialize<JsonData>(value, Consts.Json.SystemTextJsonDeserializeOptions);
                value = JsonSerializer.Serialize(json, Consts.Json.SystemTextJsonSerializerOptions);
                return json;
            }
            catch (Exception ex)
            {
            }
            return new();
        }
    }
}
