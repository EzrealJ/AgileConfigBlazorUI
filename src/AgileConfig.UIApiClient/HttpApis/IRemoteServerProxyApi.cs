using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IRemoteServerProxyApi : IHttpApi
    {
        /// <summary>
        /// 通知一个节点的某个客户端离线
        /// </summary>
        /// <param name="address"></param>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("RemoteServerProxy/Client_Offline")]
        ITask<ApiResult> OfflineAsync(string address, string clientId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通知某个节点让所有的客户端刷新配置项
        /// </summary>
        /// <param name="address"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("RemoteServerProxy/AllClients_Reload")]
        ITask<ApiResult> ReloadAsync(string address, CancellationToken cancellationToken = default);

        /// <summary>
        /// 通知某个节点个某个客户端刷新配置项
        /// </summary>
        /// <param name="address"></param>
        /// <param name="clientId"></param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Success</returns>
        [HttpPost("RemoteServerProxy/Client_Reload")]
        ITask<ApiResult> ReloadAsync(string address, string clientId, CancellationToken cancellationToken = default);

    }
}
