using System.Text.Json;
using System;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Model;
using AgileConfig.BlazorUI.Pages;
using AgileConfig.UIApiClient;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IO;
using System.Text;
using System.IO.Pipes;

namespace AgileConfig.BlazorUI.Components.Config
{
    public class ConfigExportParameter
    {
        public string AppId { get; set; }
        public string ENV { get; set; }
    }
    public partial class ConfigExport
    {
        public bool Visible { get; set; }
        [Parameter]
        public ConfigExportParameter Para { get; set; }
        [Inject]
        public IConfigApi ConfigApi { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }


        private string _value = string.Empty;

        protected override async Task OnParametersSetAsync()
        {
            _value = await ConfigApi.ExportJsonAsync(Para.AppId, Para.ENV);
            await CheckJsonAsync(_value);
        }
        private async Task CheckJsonAsync(string value)
        {
            try
            {
                var json = JsonSerializer.Deserialize<JsonData>(value, Consts.Json.SystemTextJsonDeserializeOptions);
                _value = JsonSerializer.Serialize(json, Consts.Json.SystemTextJsonSerializerOptions);
            }
            catch (Exception ex)
            {
            }
            await Task.CompletedTask;
        }
        private void Cancel()
        {
            _value = string.Empty;
            Para = new();
            Visible = false;
        }

        private async Task SaveAsync()
        {
            string fileName = $"{Para.AppId}-{Para.ENV}.json";
            using var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(_value));
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
    }
}
