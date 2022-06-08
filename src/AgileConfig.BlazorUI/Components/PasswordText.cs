using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AgileConfig.BlazorUI.Components
{
    public class PasswordText : AntDesign.Text
    {
        private const string PASSWORD_TEXT = "******";

        private string _eyeIcon;

        private bool _visible = false;

        [Parameter]
        public bool VisibilityToggle { get; set; } = true;
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                return;
            }
            builder.OpenElement(0, "div");
            base.BuildRenderTree(builder);
            if (VisibilityToggle)
            {
                builder.OpenElement(2, "a");
                builder.AddAttribute(3, "onclick", () => ToggleVisibility());
                builder.OpenComponent<Icon>(4);
                SetIcon();
                builder.AddAttribute(5, "type", _eyeIcon);
                builder.AddAttribute(6, "theme", IconThemeType.Outline);
                builder.CloseComponent();
                builder.CloseElement();
            }
            builder.CloseElement();
        }

        protected void ChildContentBuilder(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "span");
            if (_visible)
            {
                builder.AddContent(1, CopyConfig?.Text);
            }
            else
            {
                builder.AddContent(1, PASSWORD_TEXT);
            }
            builder.CloseElement();
        }

        protected override void OnInitialized()
        {

            base.OnInitialized();
            ChildContent = ChildContentBuilder;
        }
        private void SetIcon()
        {
            if (_visible)
            {
                _eyeIcon = "eye";

            }
            else
            {
                _eyeIcon = "eye-invisible";
            }
        }

        private void ToggleVisibility()
        {
            if (!VisibilityToggle)
            {
                return;
            }

            SetIcon();
            ChildContent = ChildContentBuilder;
            _visible = !_visible;
        }
    }
}
