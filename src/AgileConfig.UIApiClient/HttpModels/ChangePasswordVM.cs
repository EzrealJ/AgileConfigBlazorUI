using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class ChangePasswordVM
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [JsonPropertyName("oldPassword")]
        public string OldPassword { get; set; }

    }
}