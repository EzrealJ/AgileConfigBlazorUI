using System.ComponentModel;

namespace AgileConfig.UIApiClient
{
    public enum EnumRole
    {
        [Description("超级管理员")]
        SuperAdmin = 0,
        [Description("管理员")]
        Admin = 1,
        [Description("操作员")]
        NormalUser = 2,

    }
}
