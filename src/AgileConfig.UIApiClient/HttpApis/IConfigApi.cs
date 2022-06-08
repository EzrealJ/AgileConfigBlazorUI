using AgileConfig.UIApiClient.HttpResults;
using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IConfigApi : IHttpApi
    {
        [HttpPost("Config/Add")]
        ITask<ApiResult> AddAsync(string env, [JsonContent] ConfigVM body, CancellationToken cancellationToken = default);

        [HttpPost("Config/AddRange")]
        ITask<ApiResult> AddRangeAsync(string env, [JsonContent] IEnumerable<ConfigVM> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/Edit")]
        ITask<ApiResult> EditAsync(string env, [JsonContent] ConfigVM body, CancellationToken cancellationToken = default);

        [HttpGet("Config/All")]
        Task AllAsync(string env, CancellationToken cancellationToken = default);

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
        ITask<PageResult<ConfigVM>> SearchAsync(string appId, string group, string key, OnlineStatus? onlineStatus, string sortField, string ascOrDesc, string env, int pageSize, int current, CancellationToken cancellationToken = default);

        [HttpGet("Config/Get")]
        Task GetAsync(string id, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/Delete")]
        ITask<ApiResult> DeleteAsync(string id, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/DeleteSome")]
        ITask<ApiResult> DeleteSomeAsync(string env, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/Rollback")]
        ITask<ApiResult> RollbackAsync(string publishTimelineId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/ConfigPublishedHistory")]
        ITask<ApiResult<List<ConfigItemPublishedLog>>> ConfigPublishedHistoryAsync(string configId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 发布所有待发布的配置项
        /// </summary>
        /// <param name="env"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("Config/Publish")]
        ITask<ApiResult> PublishAsync(string env, [JsonContent] PublishLogVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 预览上传的json文件
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/PreViewJsonFile")]
        ITask<ApiResult> PreViewJsonFileAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 导出json文件
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/ExportJson")]
        ITask<string> ExportJsonAsync(string appId, string env, CancellationToken cancellationToken = default);

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
        ITask<ApiResult<List<ConfigPublishedLog>>> PublishHistoryAsync(string appId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/CancelEdit")]
        Task CancelEditAsync(string configId, string env, CancellationToken cancellationToken = default);

        [HttpGet("Config/CancelSomeEdit")]
        ITask<ApiResult> CancelSomeEditAsync(string env, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpPost("Config/SyncEnv")]
        ITask<ApiResult> SyncEnvAsync(string appId, string currentEnv, [JsonContent] IEnumerable<string> body, CancellationToken cancellationToken = default);

        [HttpGet("Config/GetKvList")]
        ITask<ApiResult<List<KeyValuePair<string, string>>>> GetKvListAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取json格式的配置
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Config/GetJson")]
        ITask<ApiResult<string>> GetJsonAsync(string appId, string env, CancellationToken cancellationToken = default);

        [HttpPost("Config/SaveJson")]
        ITask<ApiResult> SaveJsonAsync(string appId, string env, [JsonContent] SaveJsonVM body, CancellationToken cancellationToken = default);

        [HttpPost("Config/SaveKvList")]
        ITask<ApiResult> SaveKvListAsync(string appId, string env, [JsonContent] SaveKVListVM body, CancellationToken cancellationToken = default);

    }
}
