using System.Text.Json.Serialization;
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