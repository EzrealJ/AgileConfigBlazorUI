using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class ResetPassword
    {
        private Form<ChangePasswordVM> _form;
        private ChangePasswordVM _changePasswordVM = new();
        [Inject]
        public IAdminApi AdminApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        private async Task SubmitAsync()
        {
            if (!_form.Validate())
            {
                return;
            }

            var config = new MessageConfig()
            {
                Content = "修改中...",
                Key = $"{nameof(ResetPassword)}-{nameof(SubmitAsync)}"
            };
            _ = MessageService.Loading(config);
            var res = await AdminApi.ChangePasswordAsync(_changePasswordVM);
            if (res.Success)
            {
                config.Content = "修改成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"修改失败,{res.Message}";
                await MessageService.Error(config);
            }
        }



    }
}
