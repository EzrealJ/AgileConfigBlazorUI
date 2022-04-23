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
    public interface IConfigApi : IHttpApi
    {
        /// <summary>
        /// 根据appid查所有发布的配置项 , 包括继承过来的配置项.
        /// 注意： 这个接口用的不是用户名密码的认证，用的是appid + secret的认证
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/Config/app/{appId}")]
        ITask<List<ApiConfigVM>> AppAsync([Required] string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据应用id查找配置，这些配置有可能是未发布的配置 。请跟 config/app/{appId} 接口加以区分。
        /// </summary>
        /// <param name="appId">应用id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/Config")]
        ITask<List<ApiConfigVM>> ConfigAllAsync(string appId, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加一个配置项
        /// </summary>
        /// <param name="env">环境</param>
        /// <param name="body">配置模型</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("api/Config")]
        Task ConfigAsync(string env, [JsonContent] ApiConfigVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据编号获取配置项的详情
        /// </summary>
        /// <param name="id">配置id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/Config/{id}")]
        ITask<ApiConfigVM> Config2Async([Required] string id, string env, CancellationToken cancellationToken = default);

        /// <summary>
        /// 编辑一个配置
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="env">环境</param>
        /// <param name="body">模型</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPut("api/Config/{id}")]
        Task Config3Async([Required] string id, string env, [JsonContent] ApiConfigVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除一个配置
        /// </summary>
        /// <param name="id">配置id</param>
        /// <param name="env">环境</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpDelete("api/Config/{id}")]
        Task Config4Async([Required] string id, string env, CancellationToken cancellationToken = default);

    }
}