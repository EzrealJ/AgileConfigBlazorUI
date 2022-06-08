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
