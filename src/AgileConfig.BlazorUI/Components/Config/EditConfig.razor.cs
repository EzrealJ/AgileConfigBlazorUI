using System.Collections.Generic;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Config
{
    public partial class EditConfig
    {
        private IEnumerable<UserVM> _adminOptions = new List<UserVM>();
        private Form<ConfigVM> _form;
        private IEnumerable<string> _groupOptions = new List<string>();
        private IEnumerable<AppVM> _publicAppOptions = new List<AppVM>();
        [Parameter]
        public ConfigVM CurrentObject { get; set; } = new();

        [Parameter]
        public EnumEditType EditType { get; set; }

        public string ENV { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }

        [Parameter]
        public EventCallback OnCompleted { get; set; }
        [Inject]
        public IUserApi UserApi { get; set; }

        public bool Visible { get; set; }
        [Inject]
        private IConfigApi ConfigApi { get; set; }

        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        protected override async Task OnParametersSetAsync()
        {
            if (Visible)
            {
                await LoadDataAsync();
            }
        }



        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
        private async Task OnOkAsync(MouseEventArgs e)
        {
            var config = new MessageConfig()
            {
                Content = $"正在{Title}...",
                Key = $"{nameof(EditType)}-{CurrentObject.Key}"
            };
            ApiResult res = new();
            if (EditType == EnumEditType.Add)
            {
                res = await ConfigApi.AddAsync(ENV, CurrentObject);
            }
            if (EditType == EnumEditType.Edit)
            {
                res = await ConfigApi.EditAsync(ENV, CurrentObject);
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
