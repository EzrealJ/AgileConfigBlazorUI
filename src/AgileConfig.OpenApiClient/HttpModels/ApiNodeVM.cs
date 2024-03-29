﻿using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class ApiNodeVM
    {
        /// <summary>节点地址</summary>
        [JsonPropertyName("address")]
        public string Address { get; set; }

        /// <summary>备注</summary>
        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("status")]
        public NodeStatus Status { get; set; }

        /// <summary>最后响应时间</summary>
        [JsonPropertyName("lastEchoTime")]
        public System.DateTimeOffset? LastEchoTime { get; set; }

    }
}