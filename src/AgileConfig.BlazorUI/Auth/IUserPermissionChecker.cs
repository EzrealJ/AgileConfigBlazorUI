using System.Collections.Generic;

namespace AgileConfig.BlazorUI.Auth
{
    public interface IUserPermissionChecker
    {
        bool CheckUserPermission(IEnumerable<string> functions, string judgeKey, string appId);
    }
}
