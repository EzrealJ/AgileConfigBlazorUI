using AgileConfig.UIApiClient.HttpModels;
using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface ISysLogApi : IHttpApi
    {
        [HttpGet("SysLog/Search")]
        ITask<PageResult<SysLogVM>> SearchAsync(string appId, SysLogType? logType, System.DateTimeOffset? startTime, System.DateTimeOffset? endTime, int current, int pageSize, CancellationToken cancellationToken = default);

    }
}
