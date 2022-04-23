using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.OpenApiClient
{
    /// <summary>restful api 返回的 app  模型</summary>
    public class ApiAppVM
    {
        /// <summary>是否可继承</summary>
        [JsonPropertyName("inheritanced")]
        public bool Inheritanced { get; set; }

        /// <summary>id</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>name</summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>密钥</summary>
        [JsonPropertyName("secret")]
        public string Secret { get; set; }

        /// <summary>是否启用</summary>
        [JsonPropertyName("enabled")]
        public bool? Enabled { get; set; }

        /// <summary>关联的app</summary>
        [JsonPropertyName("inheritancedApps")]
        public List<string> InheritancedApps { get; set; }

        /// <summary>管理员</summary>
        [JsonPropertyName("appAdmin")]
        public string AppAdmin { get; set; }

        [JsonPropertyName("group")]
        public string Group { get; set; }

        [JsonPropertyName("createTime")]
        public System.DateTimeOffset CreateTime { get; set; }

    }
}