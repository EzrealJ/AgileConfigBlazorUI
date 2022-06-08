using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace AgileConfig.UIApiClient
{
    public class ServerNodeVM
    {

        [Required]
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;


        [StringLength(50)]
        public string Remark { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public NodeStatus Status { get; set; }

        public DateTimeOffset CreateTime { get; set; }


        public DateTimeOffset? LastEchoTime { get; set; }

    }
}
