using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.App
{
    public partial class EditApp
    {
        private Form<AppVM> _form;
        [Parameter]
        public EnumEditType EditType { get; set; }
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        public bool Visible { get; set; }
        [Parameter]
        public AppVM CurrentObject { get; set; } = new AppVM();

        [Inject]
        private IAppApi AppApi { get; set; }
        [Inject]
        public IUserApi UserApi { get; set; }
        [Inject]
        public MessageService MessageService { get; set; }


        public IEnumerable<string> InheritancedApp
        {
            get => CurrentObject.InheritancedApps;
            set => CurrentObject.InheritancedApps = value.ToList();
        }

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
            var res = await AppApi.GetAppGroupsAsync();
            _groupOptions = res.Data ?? Array.Empty<string>();
            var res1 = await AppApi.InheritancedAppsAsync(CurrentObject?.Id ?? string.Empty);
            _publicAppOptions = res1.Data ?? Array.Empty<AppVM>();
            var res2 = await UserApi.AdminUsersAsync();
            _adminOptions = res2.Data ?? Array.Empty<UserVM>();
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
                Key = $"{nameof(EditType)}-{CurrentObject.Name}"
            };
            ApiResult res = new();
            if (EditType == EnumEditType.Add)
            {
                res = await AppApi.AddAsync(CurrentObject);
            }
            if (EditType == EnumEditType.Edit)
            {
                res = await AppApi.EditAsync(CurrentObject);
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



        private void AddItem(MouseEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                return;
            }
            if (_groupOptions.Contains(_name))
            {
                return;
            }
            var temp = _groupOptions.ToList();
            temp.Add(_name);
            _groupOptions = temp.ToArray();
            _name = string.Empty;
        }
    }
}
