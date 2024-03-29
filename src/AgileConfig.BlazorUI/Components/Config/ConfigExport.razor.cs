﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Helpers;
using AgileConfig.BlazorUI.Model;
using AgileConfig.UIApiClient;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AgileConfig.BlazorUI.Components.Config
{
    public partial class ConfigExport
    {
        private string _value = string.Empty;
        [Inject]
        public IConfigApi ConfigApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public ConfigExportParameter Para { get; set; }

        public bool Visible { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            if (!Visible)
            {
                return;
            }
            await LoadDataAsync();
        }

        private void Cancel()
        {
            _value = string.Empty;
            Para = new();
            Visible = false;
        }


        private async Task LoadDataAsync()
        {
            _value = await ConfigApi.ExportJsonAsync(Para.AppId, Para.ENV);
            JsonDataHelper.SerializeJson(ref _value);
        }
        private async Task SaveAsync()
        {
            string fileName = $"{Para.AppId}-{Para.ENV}.json";
            using var fileStream = new MemoryStream(Encoding.UTF8.GetBytes(_value));
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            Visible = false;
        }
    }

    public class ConfigExportParameter
    {
        public string AppId { get; set; }
        public string ENV { get; set; }
    }
}
