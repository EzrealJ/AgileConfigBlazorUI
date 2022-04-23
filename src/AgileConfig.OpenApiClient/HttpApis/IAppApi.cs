using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;
namespace AgileConfig.OpenApiClient
{
    [LoggingFilter]
    public interface IAppApi : IHttpApi
    {
        /// <summary>
        /// 获取所有应用
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/App")]
        ITask<List<ApiAppVM>> AppAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="body">应用模型</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("api/App")]
        Task AppAsync([JsonContent] ApiAppVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据id获取应用
        /// </summary>
        /// <param name="id">应用id</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/App/{id}")]
        ITask<ApiAppVM> App2Async([Required] string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 编辑应用
        /// </summary>
        /// <param name="id">应用id</param>
        /// <param name="body">编辑后的应用模型</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPut("api/App/{id}")]
        Task App3Async([Required] string id, [JsonContent] ApiAppVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除应用
        /// </summary>
        /// <param name="id">应用id</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpDelete("api/App/{id}")]
        Task App4Async([Required] string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发布某个应用的待发布配置项
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("api/App/publish")]
        Task PublishAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询某个应用的发布历史
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/App/Publish_History")]
        ITask<List<ApiPublishTimelineVM>> HistoryAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 回滚某个应用的发布版本，回滚到 historyId 指定的时刻
        /// </summary>
        /// <param name="historyId">发布历史</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("api/App/rollback")]
        Task RollbackAsync(string historyId, string env, CancellationToken cancellationToken = default);

    }
}