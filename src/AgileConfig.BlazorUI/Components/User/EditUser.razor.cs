using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.BlazorUI.Extensions;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.User
{
    public partial class EditUser
    {
        private Form<UserVM> _form;
        [Parameter]
        public UserVM CurrentObject { get; set; } = new UserVM();

        [Parameter]
        public EnumEditType EditType { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        [Parameter]
        public EventCallback OnCompleted { get; set; }
        [Inject]
        public IUserApi UserApi { get; set; }

        public bool Visible { get; set; }
        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
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
