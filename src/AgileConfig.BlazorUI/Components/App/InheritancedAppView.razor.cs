using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Components.App
{
    public partial class InheritancedAppView
    {
        [Parameter]
        public string AppId { get; set; }

        [Parameter]
        public string AppName { get; set; }

        [Parameter]
        public List<string> InheritancedAppNames { get; set; } = new();

        public string Title => $"{AppName}-应用关联";
        public bool Visible { get; set; }
        private void Cancel(MouseEventArgs e)
        {
            AppId = string.Empty;
            AppName = string.Empty;
            InheritancedAppNames = new(0);
            Visible = false;
        }
    }
}
