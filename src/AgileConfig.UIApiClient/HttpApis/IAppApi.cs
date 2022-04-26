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
    public interface IAppApi : IHttpApi
    {
        [HttpGet("App/Search")]
        ITask<PageResult<AppListVM>> SearchAsync(string name, string id, string group, string sortField, string ascOrDesc, bool? tableGrouped, int current, int pageSize, CancellationToken cancellationToken = default);

        [HttpPost("App/Add")]
        Task AddAsync([JsonContent] AppVM body, CancellationToken cancellationToken = default);

        [HttpPost("App/Edit")]
        Task EditAsync([JsonContent] AppVM body, CancellationToken cancellationToken = default);

        [HttpGet("App/All")]
        Task AllAsync(CancellationToken cancellationToken = default);

        [HttpGet("App/Get")]
        Task GetAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 在启动跟禁用之间进行切换
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("App/DisableOrEanble")]
        Task DisableOrEanbleAsync(string id, CancellationToken cancellationToken = default);

        [HttpPost("App/Delete")]
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有可以继承的app
        /// </summary>
        /// <param name="currentAppId"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("App/InheritancedApps")]
        Task InheritancedAppsAsync(string currentAppId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 保存app的授权信息
        /// </summary>
        /// <param name="body"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("App/SaveAppAuth")]
        Task SaveAppAuthAsync([JsonContent] AppAuthVM body, CancellationToken cancellationToken = default);

        [HttpGet("App/GetUserAppAuth")]
        Task GetUserAppAuthAsync(string appId, CancellationToken cancellationToken = default);

        [HttpGet("App/GetAppGroups")]
        Task GetAppGroupsAsync(CancellationToken cancellationToken = default);

    }
}
