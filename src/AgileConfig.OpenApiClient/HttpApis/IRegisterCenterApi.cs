using System.ComponentModel.DataAnnotations;
using WebApiClientCore;
using WebApiClientCore.Attributes;
namespace AgileConfig.OpenApiClient
{
    [LoggingFilter]
    public interface IRegisterCenterApi : IHttpApi
    {
        [HttpPost("api/RegisterCenter")]
        ITask<RegisterResultVM> RegisterCenterAsync([JsonContent] RegisterServiceInfoVM body, CancellationToken cancellationToken = default);

        [HttpDelete("api/RegisterCenter/{id}")]
        ITask<RegisterResultVM> RegisterCenter2Async([Required] string id, [JsonContent] RegisterServiceInfoVM body, CancellationToken cancellationToken = default);

        [HttpPost("api/RegisterCenter/heartbeat")]
        ITask<HeartbeatResultVM> HeartbeatAsync([JsonContent] HeartbeatParam body, CancellationToken cancellationToken = default);

        [HttpGet("api/RegisterCenter/services")]
        ITask<List<ServiceInfoVM>> ServicesAsync(CancellationToken cancellationToken = default);

        [HttpGet("api/RegisterCenter/services/online")]
        ITask<List<ServiceInfoVM>> OnlineAsync(CancellationToken cancellationToken = default);

        [HttpGet("api/RegisterCenter/services/offline")]
        ITask<List<ServiceInfoVM>> OfflineAsync(CancellationToken cancellationToken = default);

    }
}