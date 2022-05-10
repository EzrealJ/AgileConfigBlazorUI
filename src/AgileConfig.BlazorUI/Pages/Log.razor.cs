using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AgileConfig.BlazorUI.Extensions;
using AgileConfig.UIApiClient;
using AgileConfig.UIApiClient.HttpModels;
using AntDesign;
using Microsoft.AspNetCore.Components;

namespace AgileConfig.BlazorUI.Pages
{
    public partial class Log
    {
        class FormClass
        {
            public string AppId { get; set; }
            public SysLogType LogType { get; set; }
            public DateTimeOffset? StartTime { get; set; }
            public DateTimeOffset? EndTime { get; set; }
        }

        private FormClass _formClass = new();
        public string LogType
        {
            get => _formClass.LogType.GetIntValue().ToString();
            set => _formClass.LogType = Enum.Parse<SysLogType>(value);
        }

        private PageResult<SysLogVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        private IEnumerable<string> _appIdOptions = Array.Empty<string>();
        private DateTime?[] TimeRange
        {
            get => new DateTime?[] { _formClass.StartTime?.DateTime, _formClass.EndTime?.DateTime };
            set
            {
                _formClass.StartTime = value[0];
                _formClass.EndTime = value[1];

            }
        }

        [Inject]
        public ISysLogApi SysLogApi { get; set; }
        [Inject]
        public IAppApi AppApi { get; set; }
        private void ReSet() => _formClass = new();

        private async Task SearchAsync()
        {
            _dataSource = await SysLogApi.SearchAsync(
                _formClass.AppId,
                _formClass.LogType,
                _formClass.StartTime,
                _formClass.EndTime,
               current: _dataSource.Current,
               pageSize: _dataSource.PageSize);
            StateHasChanged();
        }
        private async void SearchAppAsync(string input)
        {
            var temp = await AppApi.SearchAsync(name: input, null, null, null, null, null, null, null);
            _appIdOptions = temp?.Data.Select(app => app.Id) ?? Array.Empty<string>();
            StateHasChanged();
        }

        private void OnTimeRangeChange(DateRangeChangedEventArgs args)
        {
            _formClass.StartTime = args.Dates[0];
            _formClass.EndTime = args.Dates[1];
        }
    }
}
