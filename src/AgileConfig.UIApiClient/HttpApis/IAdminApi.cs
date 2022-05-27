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
    public interface IAdminApi : IHttpApi
    {
        [HttpPost("/admin/jwt/login")]
        ITask<LoginResult> LoginAsync([JsonContent] LoginVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// is password inited ?
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Admin/PasswordInited")]
        Task<ApiResult<bool>> PasswordInitedAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 初始化密码
        /// </summary>
        /// <param name="body"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("Admin/InitPassword")]
        ITask<ApiResult> InitPasswordAsync([JsonContent] InitPasswordVM body, CancellationToken cancellationToken = default);

        [HttpPost("Admin/ChangePassword")]
        ITask<ApiResult> ChangePasswordAsync([JsonContent] ChangePasswordVM body, CancellationToken cancellationToken = default);

        [HttpPost("Admin/Logoff")]
        Task LogoffAsync(CancellationToken cancellationToken = default);

    }
}
