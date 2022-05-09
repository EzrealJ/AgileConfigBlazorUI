using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AgileConfig.UIApiClient.HttpModels;
using AgileConfig.UIApiClient.HttpResults;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using WebApiClientCore.Parameters;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IReportApi : IHttpApi
    {
        /// <summary>
        /// 获取本节点的客户端信息
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/Clients")]
        Task ClientsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取某个节点的客户端信息
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/ServerNodeClients")]
        Task ServerNodeClientsAsync(string address, CancellationToken cancellationToken = default);

        [HttpGet("Report/SearchServerNodeClients")]
        ITask<PageResult<ClientVM>> SearchServerNodeClientsAsync(string address, int? current, int? pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取App数量
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/AppCount")]
        ITask<int> AppCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取Config项目数量
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/ConfigCount")]
        ITask<int> ConfigCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取Node项目数量
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/NodeCount")]
        ITask<int> NodeCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取所有服务的状态信息
        /// </summary>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpGet("Report/RemoteNodesStatus")]
        ITask<List<RemoteNodeStatus>> RemoteNodesStatusAsync(CancellationToken cancellationToken = default);

        [HttpGet("Report/ServiceCount")]
        ITask<ServiceCountResult> ServiceCountAsync(CancellationToken cancellationToken = default);

    }
}
