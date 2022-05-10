using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AgileConfig.UIApiClient
{
    public enum SysLogType
    {
        [Description("普通")]
        Normal = 0,
        [Description("警告")]
        Warn = 1
    }
}
