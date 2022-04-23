using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.OpenApiClient
{
    public class LoginVM
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}