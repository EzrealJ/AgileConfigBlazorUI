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
    public interface INodeApi : IHttpApi
    {
        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("api/Node")]
        ITask<List<ApiNodeVM>> NodeAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="body">节点模型</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("api/Node")]
        Task NodeAsync([JsonContent] ApiNodeVM body, CancellationToken cancellationToken = default);

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="address">节点地址</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpDelete("api/Node")]
        Task Node2Async(string address, CancellationToken cancellationToken = default);

    }
}