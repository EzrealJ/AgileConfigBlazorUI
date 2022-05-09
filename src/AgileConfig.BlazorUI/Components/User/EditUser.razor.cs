using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using AgileConfig.BlazorUI.Extensions;

namespace AgileConfig.BlazorUI.Components.User
{
    public partial class EditUser
    {
        private Form<UserVM> _form;
        [Parameter]
        public EnumEditType EditType { get; set; }
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        public bool Visible { get; set; }
        [Parameter]
        public UserVM CurrentObject { get; set; } = new UserVM();

        [Inject]
        public IUserApi UserApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }
        private IEnumerable<string> UserRoles
        {
            get => CurrentObject.UserRoles?.Select(r => r.GetIntValue().ToString());
            set => CurrentObject.UserRoles = value.Select(v => Enum.Parse<EnumRole>(v)).ToList();
        }

        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
        }

        private async Task OnOkAsync(MouseEventArgs e)
        {
            _form.Validate();
            var config = new MessageConfig()
            {
                Content = $"正在{Title}...",
                Key = $"{nameof(EditType)}-{CurrentObject.UserName}"
            };
            ApiResult res = new();
            if (EditType == EnumEditType.Add)
            {
                res = await UserApi.AddAsync(CurrentObject);
            }
            if (EditType == EnumEditType.Edit)
            {
                res = await UserApi.EditAsync(CurrentObject);
            }
            if (res.Success)
            {
                config.Content = $"{Title}成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"{Title}失败,{res.Message}";
                await MessageService.Error(config);
            }
            Visible = false;
            if (OnCompleted.HasDelegate)
            {
                await OnCompleted.InvokeAsync(e);
            }
        }

    }

}
