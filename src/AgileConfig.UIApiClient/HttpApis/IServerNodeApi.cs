using AgileConfig.UIApiClient.HttpResults;
using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IServerNodeApi : IHttpApi
    {
        [HttpPost("ServerNode/Add")]
        ITask<ApiResult> AddAsync([JsonContent] ServerNodeVM body, CancellationToken cancellationToken = default);

        [HttpPost("ServerNode/Delete")]
        ITask<ApiResult> DeleteAsync([JsonContent] ServerNodeVM body, CancellationToken cancellationToken = default);

        [HttpGet("ServerNode/All")]
        ITask<ServerNodeResult> AllAsync(CancellationToken cancellationToken = default);

    }
}
