using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.UIApiClient
{
    public class InitPasswordVM
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }


        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; }

    }
}
