using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
