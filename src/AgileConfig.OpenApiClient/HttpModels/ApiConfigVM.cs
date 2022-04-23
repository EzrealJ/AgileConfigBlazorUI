using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.OpenApiClient
{
    public class ApiConfigVM
    {
        /// <summary>id</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>应用</summary>
        [JsonPropertyName("appId")]
        public string AppId { get; set; }

        /// <summary>组</summary>
        [JsonPropertyName("group")]
        public string Group { get; set; }

        /// <summary>键</summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>值</summary>
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("status")]
        public ConfigStatus Status { get; set; }

        [JsonPropertyName("onlineStatus")]
        public OnlineStatus OnlineStatus { get; set; }

        [JsonPropertyName("editStatus")]
        public EditStatus EditStatus { get; set; }

        /// <summary>描述</summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

    }
}