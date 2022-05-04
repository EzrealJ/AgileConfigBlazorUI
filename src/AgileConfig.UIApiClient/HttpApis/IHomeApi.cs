using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AgileConfig.UIApiClient.HttpResults;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;
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
