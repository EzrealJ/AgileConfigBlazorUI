using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgileConfig.UIApiClient
{
    /// <summary>
    ///    Deleted = 0,
    ///    Enabled = 1,
    /// </summary>
    public enum ConfigStatus
    {
        Deleted = 0,
        Enabled = 1,
    }

    /// <summary>
    ///    Add = 0,
    /// Edit = 1,
    ///   Deleted = 2,
    ///   Commit = 10
    /// </summary>
    public enum EditStatus
    {
        [Description("新增")]
        Add = 0,
        [Description("编辑")]
        Edit = 1,
        [Description("删除")]
        Deleted = 2,
        [Description("已提交")]
        Commit = 10
    }

    /// <summary>
    ///   WaitPublish = 0,
    /// Online = 1,
    /// </summary>
    public enum OnlineStatus
    {
        [Description("待发布")]
        WaitPublish = 0,
        [Description("已发布")]
        Online = 1,
    }

    public class ConfigVM
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("appId")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(36)]
        public string AppId { get; set; } = string.Empty;

        [JsonPropertyName("group")]
        [StringLength(100)]
        public string Group { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(4000)]
        public string Value { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("onlineStatus")]
        public OnlineStatus OnlineStatus { get; set; }

        [JsonPropertyName("status")]
        public ConfigStatus Status { get; set; }

        [JsonPropertyName("createTime")]
        public DateTime? CreateTime { get; set; }

        [JsonPropertyName("updateTime")]
        public DateTime? UpdateTime { get; set; }

        public EditStatus EditStatus { get; set; }

    }
}
