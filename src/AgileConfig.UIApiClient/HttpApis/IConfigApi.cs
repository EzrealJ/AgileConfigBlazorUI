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
    public interface IConfigApi : IHttpApi
    {
        [HttpPost("Config/Add")]
        Task Add2Async(string env, [JsonContent] ConfigVM body, CancellationToken cancellationToken = default);

        [HttpPost("Config/AddRange")]
        Task AddRangeAsync(string env, [JsonContent] IEnumerable<ConfigVM> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/Edit")]
        Task Edit2Async(string env, [JsonContent] ConfigVM body, CancellationToken cancellationToken = default);

        [HttpGet("Config/All")]
        Task All2Async(string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 按多条件进行搜索
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="group">分组</param>
        /// <param name="key">键</param>
        /// <param name="onlineStatus">在线状态</param>
        /// <param name="sortField"></param>
        /// <param name="ascOrDesc"></param>
        /// <param name="env"></param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="current">当前页</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/Search")]
        Task Search2Async(string appId, string group, string key, OnlineStatus? onlineStatus, string sortField, string ascOrDesc, string env, int pageSize, int current, CancellationToken cancellationToken = default);

        [HttpGet("Config/Get")]
        Task Get2Async(string id, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/Delete")]
        Task Delete2Async(string id, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/DeleteSome")]
        Task DeleteSomeAsync(string env, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/Rollback")]
        Task RollbackAsync(string publishTimelineId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/ConfigPublishedHistory")]
        Task ConfigPublishedHistoryAsync(string configId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发布所有待发布的配置项
        /// </summary>
        /// <param name="env"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("Config/Publish")]
        Task PublishAsync(string env, [JsonContent] PublishLogVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 预览上传的json文件
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/PreViewJsonFile")]
        Task PreViewJsonFileAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 导出json文件
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/ExportJson")]
        Task ExportJsonAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取待发布的明细
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/WaitPublishStatus")]
        Task WaitPublishStatusAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取发布详情的历史
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/PublishHistory")]
        Task PublishHistoryAsync(string appId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/CancelEdit")]
        Task CancelEditAsync(string configId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/CancelSomeEdit")]
        Task CancelSomeEditAsync(string env, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/SyncEnv")]
        Task SyncEnvAsync(string appId, string currentEnv, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpGet("Config/GetKvList")]
        Task GetKvListAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取json格式的配置
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/GetJson")]
        Task GetJsonAsync(string appId, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/SaveJson")]
        Task SaveJsonAsync(string appId, string env, [JsonContent] SaveJsonVM body, CancellationToken cancellationToken = default);

        [HttpPost("Config/SaveKvList")]
        Task SaveKvListAsync(string appId, string env, [JsonContent] SaveKVListVM body, CancellationToken cancellationToken = default);

    }
}