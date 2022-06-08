using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class HeartbeatParam
    {
        [JsonPropertyName("uniqueId")]
        public string UniqueId { get; set; }

    }
}