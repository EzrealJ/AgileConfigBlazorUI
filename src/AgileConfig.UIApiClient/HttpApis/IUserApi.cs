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
        ITask<PageResult<UserVM>> SearchAsync(string userName, string team, int current, int pageSize, CancellationToken cancellationToken = default);

        [HttpPost("User/Add")]
        ITask<ApiResult> AddAsync([JsonContent] UserVM body, CancellationToken cancellationToken = default);

        [HttpPost("User/Edit")]
        ITask<ApiResult> EditAsync([JsonContent] UserVM body, CancellationToken cancellationToken = default);

        [HttpPost("User/ResetPassword")]
        ITask<ApiResult> ResetPasswordAsync(string userId, CancellationToken cancellationToken = default);

        [HttpPost("User/Delete")]
        ITask<ApiResult> DeleteAsync(string userId, CancellationToken cancellationToken = default);

        [HttpGet("User/adminUsers")]
        ITask<ApiResult<UserVM[]>> AdminUsersAsync(CancellationToken cancellationToken = default);

        [HttpGet("User/AllUsers")]
        ITask<ApiResult<UserVM[]>> AllUsersAsync(CancellationToken cancellationToken = default);

    }
}
