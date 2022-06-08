using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class SaveJsonVM
    {
        [JsonPropertyName("json")]
        public string Json { get; set; }

    }
}