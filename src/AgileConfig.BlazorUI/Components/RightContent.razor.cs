using System.Collections.Generic;
using AgileConfig.BlazorUI.Consts;
using AntDesign;
using AntDesign.ProLayout;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Components
{
    public partial class RightContent : AntDomComponentBase
    {
        private readonly IEnumerable<AvatarMenuItem> _avatarMenuItems = new AvatarMenuItem[]
        {
            new AvatarMenuItem { Key = RoutePath.RESET_PASSWORD, IconType = "user", Option = "修改密码"},
            //new AvatarMenuItem { Key = "setting", IconType = "setting", Option = "设置"},
            new AvatarMenuItem { IsDivider = true },
            new AvatarMenuItem { Key = RoutePath.LOGOUT, IconType = "logout", Option = "退出登录"}
        };

        private readonly IDictionary<string, string> _languageIcons = new Dictionary<string, string>
        {
            ["zh-CN"] = "🇨🇳",
            ["en-US"] = "🇺🇸",
        };

        private readonly IDictionary<string, string> _languageLabels = new Dictionary<string, string>
        {
            ["zh-CN"] = "简体中文",
            ["en-US"] = "English",
        };

        private readonly string[] _locales = { "zh-CN", "en-US", };
        [Parameter] public EventCallback<MenuItem> OnLangItemSelected { get; set; }
        [Parameter] public EventCallback<MenuItem> OnUserItemSelected { get; set; }
        [Parameter] public string UserName { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        protected void SetClassMap()
        {
            ClassMapper
                .Clear()
                .Add("right");
        }
    }
}
