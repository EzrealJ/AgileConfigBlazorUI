﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

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
        public string Id { get; set; }

        [JsonPropertyName("appId")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(36)]
        public string AppId { get; set; }

        [JsonPropertyName("group")]
        [StringLength(100)]
        public string Group { get; set; }

        [JsonPropertyName("key")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        [Required(AllowEmptyStrings = true)]
        [StringLength(4000)]
        public string Value { get; set; }

        [JsonPropertyName("description")]
        [StringLength(200)]
        public string Description { get; set; }

        [JsonPropertyName("onlineStatus")]
        public OnlineStatus OnlineStatus { get; set; }

        [JsonPropertyName("status")]
        public ConfigStatus Status { get; set; }

        [JsonPropertyName("createTime")]
        public DateTimeOffset? CreateTime { get; set; }

        [JsonPropertyName("updateTime")]
        public DateTimeOffset? UpdateTime { get; set; }

        public EditStatus EditStatus { get; set; }

    }
}
