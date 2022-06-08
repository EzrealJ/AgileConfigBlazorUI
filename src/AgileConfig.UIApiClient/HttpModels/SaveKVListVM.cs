using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class SaveKVListVM
    {
        [JsonPropertyName("str")]
        public string Str { get; set; }

    }
}