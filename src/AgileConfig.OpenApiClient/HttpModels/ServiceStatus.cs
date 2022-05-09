using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
namespace AgileConfig.OpenApiClient
{
    public enum ServiceStatus
    {
        Unhealthy = 0,
        Healthy = 1
    }
}
