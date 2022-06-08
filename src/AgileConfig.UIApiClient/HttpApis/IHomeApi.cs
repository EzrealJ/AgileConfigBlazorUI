using AgileConfig.UIApiClient.HttpResults;
using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IHomeApi : IHttpApi
    {
        [HttpGet("Home/Index")]
        Task IndexAsync(CancellationToken cancellationToken = default);

        [HttpGet("Home/Current")]
        ITask<CurrentUserResult> CurrentAsync(CancellationToken cancellationToken = default);

        [HttpGet("Home/Sys")]
        ITask<HomeSysResult> SysAsync(CancellationToken cancellationToken = default);

        [HttpGet("Home/Echo")]
        Task EchoAsync(CancellationToken cancellationToken = default);

    }
}
