using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class RegisterServiceInfoVM
    {
        [JsonPropertyName("serviceId")]
        public string ServiceId { get; set; }

        [JsonPropertyName("serviceName")]
        public string ServiceName { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("metaData")]
        public List<string> MetaData { get; set; }

        [JsonPropertyName("checkUrl")]
        public string CheckUrl { get; set; }

        [JsonPropertyName("alarmUrl")]
        public string AlarmUrl { get; set; }

        [JsonPropertyName("heartBeatMode")]
        public string HeartBeatMode { get; set; }

    }
}