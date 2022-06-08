using System;
using System.Collections.Generic;
using System.Linq;
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
        private IEnumerable<string> _appIdOptions = Array.Empty<string>();

        private PageResult<SysLogVM> _dataSource = new()
        {
            Current = 1,
            PageSize = 20
        };

        private FormClass _formClass = new();

        [Inject]
        public IAppApi AppApi { get; set; }

        public string LogType
        {
            get => _formClass.LogType.GetIntValue().ToString();
            set => _formClass.LogType = Enum.Parse<SysLogType>(value);
        }

        [Inject]
        public ISysLogApi SysLogApi { get; set; }

        private DateTime?[] TimeRange
        {
            get => new DateTime?[] { _formClass.StartTime?.DateTime, _formClass.EndTime?.DateTime };
            set
            {
                _formClass.StartTime = value[0];
                _formClass.EndTime = value[1];

            }
        }

        private void OnTimeRangeChange(DateRangeChangedEventArgs args)
        {
            _formClass.StartTime = args.Dates[0];
            _formClass.EndTime = args.Dates[1];
        }

        private void ReSet() => _formClass = new();

        private async void SearchAppAsync(string input)
        {
            var temp = await AppApi.SearchAsync(name: input, null, null, null, null, null, null, null);
            _appIdOptions = temp?.Data.Select(app => app.Id) ?? Array.Empty<string>();
            StateHasChanged();
        }

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

        class FormClass
        {
            public string AppId { get; set; }
            public DateTimeOffset? EndTime { get; set; }
            public SysLogType LogType { get; set; }
            public DateTimeOffset? StartTime { get; set; }
        }
    }
}
