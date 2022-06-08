using System.Text.Json.Serialization;

namespace AgileConfig.UIApiClient
{
    public class ApiResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
    }
}
