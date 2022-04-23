using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.OpenApiClient
{
    public class ServiceInfoVM
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

        [JsonPropertyName("status")]
        public ServiceStatus Status { get; set; }

    }
}