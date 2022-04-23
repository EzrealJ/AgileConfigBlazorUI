using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.UIApiClient
{
    public class AppAuthVM
    {
        [JsonPropertyName("editConfigPermissionUsers")]
        public List<string> EditConfigPermissionUsers { get; set; }

        [JsonPropertyName("publishConfigPermissionUsers")]
        public List<string> PublishConfigPermissionUsers { get; set; }

        [JsonPropertyName("appId")]
        public string AppId { get; set; }

    }
}