using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgileConfig.UIApiClient.HttpModels;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace AgileConfig.UIApiClient
{
    [LoggingFilter]
    public interface IServiceApi : IHttpApi
    {
        [HttpGet("service/search")]
        ITask<PageResult<ServiceInfoVM>> SearchAsync(string serviceName, string serviceId, ServiceStatus? status,
            string sortField, string ascOrDesc,
            int current = 1, int pageSize = 20);
        [HttpPost("service/remove")]
        ITask<ApiResult> RemoveAsync(string id);
        [HttpPost("service/add")]
        Task<ApiResult> AddAsync([JsonContent]ServiceInfoVM currentObject);
    }
}
