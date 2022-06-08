using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AgileConfig.UIApiClient.HttpModels
{
    public enum ServiceStatus
    {
        Unhealthy = 0,
        Healthy = 1
    }

    public enum RegisterWay
    {
        Auto = 0,
        Manual = 1
    }

    public enum HeartBeatModes
    {
        [Description("无")]
        None,
        Client,
        Server,

    }
    public class ServiceInfoVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "服务Id不能为空")]
        [MaxLength(100, ErrorMessage = "服务Id长度不能超过100")]
        public string ServiceId { get; set; }

        [Required(ErrorMessage = "服务名不能为空")]
        [MaxLength(100, ErrorMessage = "服务名长度不能超过100")]
        public string ServiceName { get; set; }

        [MaxLength(100, ErrorMessage = "IP长度不能超过100")]
        public string Ip { get; set; }

        public int? Port { get; set; }

        public string MetaData { get; set; }

        public ServiceStatus Status { get; set; }

        public DateTime? RegisterTime { get; set; }

        public DateTime? LastHeartBeat { get; set; }

        [Required(ErrorMessage = "健康检测模式不能为空")]
        [MaxLength(10, ErrorMessage = "健康检测模式长度不能超过10位")]
        public string HeartBeatMode { get; set; } = HeartBeatModes.None.ToString();

        [MaxLength(2000, ErrorMessage = "检测URL长度不能超过2000")]
        public string CheckUrl { get; set; }

        [MaxLength(2000, ErrorMessage = "告警URL长度不能超过2000")]
        public string AlarmUrl { get; set; }
    }
}
