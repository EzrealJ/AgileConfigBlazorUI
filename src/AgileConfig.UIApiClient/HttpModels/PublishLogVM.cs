using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class PublishLogVM
    {
        [JsonPropertyName("appId")]
        public string AppId { get; set; }

        [JsonPropertyName("log")]
        public string Log { get; set; }

    }
}