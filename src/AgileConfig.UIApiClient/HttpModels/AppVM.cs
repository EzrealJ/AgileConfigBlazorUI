using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class AppVM
    {
        [JsonPropertyName("id")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(36)]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(50)]
        public string Name { get; set; }

        [JsonPropertyName("group")]
        [StringLength(50)]
        public string Group { get; set; }

        [JsonPropertyName("secret")]
        [StringLength(36)]
        public string Secret { get; set; }

        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }

        [JsonPropertyName("inheritanced")]
        public bool Inheritanced { get; set; }

        [JsonPropertyName("inheritancedApps")]
        public List<string> InheritancedApps { get; set; } = new();

        [JsonPropertyName("inheritancedAppNames")]
        public List<string> InheritancedAppNames { get; set; } = new();

        [JsonPropertyName("appAdmin")]
        public string AppAdmin { get; set; }

        [JsonPropertyName("appAdminName")]
        public string AppAdminName { get; set; }

        [JsonPropertyName("createTime")]
        public System.DateTimeOffset CreateTime { get; set; }

    }
}
