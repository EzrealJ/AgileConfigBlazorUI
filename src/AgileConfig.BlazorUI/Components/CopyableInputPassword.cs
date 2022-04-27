using System;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AgileConfig.BlazorUI.Components
{
    public class CopyableInputPassword : InputPassword
    {
        [Parameter]
        public bool Copyable { get; set; } = true;
        [Parameter]
        public TypographyCopyableConfig CopyConfig { get; set; } = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            AddOnAfter = AddCopyable;
        }

        protected  void AddCopyable(RenderTreeBuilder builder)
        {
            if (!Copyable)
            {
                return;
            }
            builder.OpenElement(0, "a");
            builder.AddAttribute(1, "onclick", (Action)(async () => await Copy()));
            builder.OpenComponent<Icon>(2);
            builder.AddAttribute(3, "Type", "copy");
            builder.AddAttribute(4, "Theme", IconThemeType.Outline);
            builder.CloseComponent();
            builder.CloseElement();
        }

        public async Task Copy()
        {
            if (!Copyable)
            {
                return;
            }
            if (string.IsNullOrEmpty(CopyConfig?.Text))
            {
                await this.JsInvokeAsync<object>(JSInteropConstants.CopyElement, Ref);
            }
            else
            {
                await this.JsInvokeAsync<object>(JSInteropConstants.Copy, CopyConfig.Text);
            }
            CopyConfig?.OnCopy?.Invoke();
        }
    }
}
