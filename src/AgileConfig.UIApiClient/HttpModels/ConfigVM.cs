using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.UIApiClient
{
    public class ConfigVM
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("appId")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(36)]
        public string AppId { get; set; }

        [JsonPropertyName("group")]
        [StringLength(100)]
        public string Group { get; set; }

        [JsonPropertyName("key")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(4000)]
        public string Value { get; set; }

        [JsonPropertyName("description")]
        [StringLength(200)]
        public string Description { get; set; }

        [JsonPropertyName("onlineStatus")]
        public OnlineStatus OnlineStatus { get; set; }

        [JsonPropertyName("status")]
        public ConfigStatus Status { get; set; }

    }
}