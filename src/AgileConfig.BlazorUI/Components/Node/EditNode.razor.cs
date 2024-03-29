﻿using System.Threading.Tasks;
using AgileConfig.BlazorUI.Enums;
using AgileConfig.UIApiClient;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.Node
{
    public partial class EditNode
    {
        private Form<ServerNodeVM> _form;

        //不支持编辑,不作为参数
        //[Parameter] 
        public ServerNodeVM CurrentObject { get; set; } = new ServerNodeVM();

        [Parameter]
        public EnumEditType EditType { get; set; }
        [Parameter]
        public EventCallback OnCompleted { get; set; }
        public bool Visible { get; set; }
        [Inject]
        private MessageService MessageService { get; set; }

        [Inject]
        private IServerNodeApi ServerNodeApi { get; set; }

        private string Title => EditType == EnumEditType.Add ? "新增" : "编辑";
        private async Task AddAsync(MouseEventArgs e)
        {
            _form.Validate();
            var config = new MessageConfig()
            {
                Content = "正在添加...",
                Key = $"{nameof(AddAsync)}-{CurrentObject.Address}"
            };
            _ = MessageService.Loading(config, 1);
            var res = await ServerNodeApi.AddAsync(CurrentObject);
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
