using System;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.BlazorUI.Model;
using AgileConfig.BlazorUI.Services;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.AgileConfigServer
{
    public partial class EditAgileConfigServer
    {
        private Form<AgileConfigServerSetting> _form;

        //不支持编辑,不作为参数
        //[Parameter] 
        public AgileConfigServerSetting CurrentObject { get; set; } = new AgileConfigServerSetting();

        [Parameter]
        public EnumEditType EditType { get; set; }
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        public bool Visible { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }

        [Inject]
        private AgileConfigServerProvider AgileConfigServerProvider { get; set; }

        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        private async Task OKAsync(MouseEventArgs e)
        {
            if (EditType == EnumEditType.Add)
                await AddAsync(e);
            else
                await UpdateAsync(e);
        }
        private async Task AddAsync(MouseEventArgs e)
        {
            _form.Validate();
            var config = new MessageConfig()
            {
                Content = "正在添加...",
                Key = $"{nameof(AddAsync)}-{CurrentObject.Url}"
            };
            _ = MessageService.Loading(config, 1);
            try
            {
                await AgileConfigServerProvider.AddAsync(CurrentObject);
                config.Content = "添加成功";
                await MessageService.Success(config);
            }
            catch (Exception ex)
            {
                config.Content = $"添加失败,{ex.Message}";
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
        private async Task UpdateAsync(MouseEventArgs e)
        {
            _form.Validate();
            var config = new MessageConfig()
            {
                Content = "正在修改...",
                Key = $"{nameof(UpdateAsync)}-{CurrentObject.Url}"
            };
            _ = MessageService.Loading(config, 1);
            try
            {
                await AgileConfigServerProvider.UpdateAsync(CurrentObject);
                config.Content = "修改成功";
                await MessageService.Success(config);
            }
            catch (Exception ex)
            {
                config.Content = $"修改失败,{ex.Message}";
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
