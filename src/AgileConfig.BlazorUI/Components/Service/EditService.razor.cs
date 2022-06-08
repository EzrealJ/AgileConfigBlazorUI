using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpModels;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Service
{
    public partial class EditService
    {

        private Form<ServiceInfoVM> _form;
        //[Parameter]
        public ServiceInfoVM CurrentObject { get; set; } = new();

        public EnumEditType EditType { get; private set; } = EnumEditType.Add;
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        public bool Visible { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }

        [Inject]
        private IServiceApi ServiceApi { get; set; }

        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        private async Task AddAsync(MouseEventArgs e)
        {
            _form.Validate();
            var config = new MessageConfig()
            {
                Content = "正在添加...",
                Key = $"{nameof(AddAsync)}-{CurrentObject.ServiceName}"
            };
            _ = MessageService.Loading(config, 1);
            ApiResult res = await ServiceApi.AddAsync(CurrentObject);
            if (res.Success)
            {
                config.Content = "添加成功";
                await MessageService.Success(config);
            }
            else
            {
                config.Content = $"添加失败,{res.Message}";
                await MessageService.Error(config);
            }
            Visible = false;
            if (OnCompleted.HasDelegate)
            {
                await OnCompleted.InvokeAsync(e);
            }
        }

        private void Cancel(MouseEventArgs e)
        {
            _form.Reset();
            Visible = false;
        }
        private Task UpdateAsync(MouseEventArgs e) => throw new System.NotImplementedException();
    }
}
