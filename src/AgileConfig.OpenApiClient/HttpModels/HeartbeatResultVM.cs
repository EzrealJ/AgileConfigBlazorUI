using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class HeartbeatResultVM
    {
        [JsonPropertyName("module")]
        public string Module { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

    }
}