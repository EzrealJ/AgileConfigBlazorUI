using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class WebsocketAction
    {
        [JsonPropertyName("module")]
        public string Module { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

    }
}