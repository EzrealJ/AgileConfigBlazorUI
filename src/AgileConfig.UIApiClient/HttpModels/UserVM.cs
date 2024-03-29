﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class UserVM
    {
        [JsonPropertyName("id")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(36)]
        public string Id { get; set; }

        [JsonPropertyName("userName")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string Password { get; set; }

        [JsonPropertyName("team")]
        [StringLength(50)]
        public string Team { get; set; }

        [JsonPropertyName("userRoles")]
        public List<EnumRole> UserRoles { get; set; }

        [JsonPropertyName("userRoleNames")]
        public List<string> UserRoleNames { get; set; }

        [JsonPropertyName("status")]
        public UserStatus Status { get; set; }

    }
}