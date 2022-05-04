using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace AgileConfig.BlazorUI.Components.Config
{
    public partial class EditConfig
    {
        private Form<ConfigVM> _form;
        [Parameter]
        public string ENV { get; set; }

        [Parameter]
        public EnumEditType EditType { get; set; }
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        public bool Visible { get; set; }
        [Parameter]
        public ConfigVM CurrentObject { get; set; } = new();

        [Inject]
        private IConfigApi ConfigApi { get; set; }
        [Inject]
        public IUserApi UserApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }


        private IEnumerable<string> _groupOptions = new List<string>();
        private IEnumerable<AppVM> _publicAppOptions = new List<AppVM>();
        private IEnumerable<UserVM> _adminOptions = new List<UserVM>();


        protected override async Task OnParametersSetAsync()
        {
            if (Visible)
            {
                await LoadDataAsync();
            }
        }



        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }

        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
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
                res = await ConfigApi.AddAsync(ENV,CurrentObject);
            }
            if (EditType == EnumEditType.Edit)
            {
                res = await ConfigApi.EditAsync(ENV,CurrentObject);
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
