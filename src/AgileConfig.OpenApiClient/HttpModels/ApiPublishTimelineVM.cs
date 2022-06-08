using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class ApiPublishTimelineVM
    {
        /// <summary>编号</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>应用id</summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; }

        /// <summary>发布时间</summary>
        [JsonPropertyName("publishTime")]
        public System.DateTimeOffset? PublishTime { get; set; }

        /// <summary>发布者</summary>
        [JsonPropertyName("publishUserId")]
        public string PublishUserId { get; set; }

        /// <summary>发布版本序号</summary>
        [JsonPropertyName("version")]
        public int Version { get; set; }

        /// <summary>发布日志</summary>
        [JsonPropertyName("log")]
        public string Log { get; set; }

        /// <summary>环境</summary>
        [JsonPropertyName("env")]
        public string Env { get; set; }

    }
}