using System.Text.Json.Serialization;
namespace AgileConfig.OpenApiClient
{
    public class RegisterResultVM
    {
        [JsonPropertyName("uniqueId")]
        public string UniqueId { get; set; }

    }
}