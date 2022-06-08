using System.Text.Json.Serialization;
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
