using AgileConfig.BlazorUI.Consts;

namespace AgileConfig.BlazorUI.Localizations
{
    public static class ModuleName
    {
        public static string GetByRoutePath(string path)
        {
            return path switch
            {
                RoutePath.APP => "应用",
                RoutePath.LOG => "日志",
                RoutePath.CLIENT => "客户端",
                RoutePath.HOME => "主页",
                RoutePath.SERVICE => "服务",
                RoutePath.USER => "用户",
                RoutePath.NODE => "节点",
                RoutePath.LOGIN => "登录",
                _ => ""
            };
        }
    }
}
