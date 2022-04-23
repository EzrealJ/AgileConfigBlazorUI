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
    public interface IServerNodeApi : IHttpApi
    {
        [HttpPost("ServerNode/Add")]
        Task AddAsync([JsonContent] ServerNodeVM body, CancellationToken cancellationToken = default);

        [HttpPost("ServerNode/Delete")]
        ITask<ApiResult> DeleteAsync([JsonContent] ServerNodeVM body, CancellationToken cancellationToken = default);

        [HttpGet("ServerNode/All")]
        ITask<ServerNodeResult> AllAsync(CancellationToken cancellationToken = default);

    }
}
