using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IUserApi : IHttpApi
    {
        [HttpGet("User/Search")]
        Task Search4Async(string userName, string team, int current, int pageSize, CancellationToken cancellationToken = default);

        [HttpPost("User/Add")]
        Task Add4Async([JsonContent] UserVM body, CancellationToken cancellationToken = default);

        [HttpPost("User/Edit")]
        Task Edit3Async([JsonContent] UserVM body, CancellationToken cancellationToken = default);

        [HttpPost("User/ResetPassword")]
        Task ResetPasswordAsync(string userId, CancellationToken cancellationToken = default);

        [HttpPost("User/Delete")]
        Task Delete4Async(string userId, CancellationToken cancellationToken = default);

        [HttpGet("User/adminUsers")]
        ITask<ApiResult<UserVM[]>> AdminUsersAsync(CancellationToken cancellationToken = default);

        [HttpGet("User/AllUsers")]
        ITask<ApiResult<UserVM[]>> AllUsersAsync(CancellationToken cancellationToken = default);

    }
}
