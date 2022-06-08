using System.Text.Json.Serialization;
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
