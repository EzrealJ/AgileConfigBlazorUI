using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.UIApiClient
{
    public class ServerNodeVM
    {

        [Required(AllowEmptyStrings = true)]
        [StringLength(100)]
        public string Address { get; set; }


        [StringLength(50)]
        public string Remark { get; set; }

        [JsonPropertyName("status")]
        public NodeStatus Status { get; set; }

        public DateTimeOffset CreateTime { get; set; }


        public DateTimeOffset? LastEchoTime { get; set; }

    }
}
