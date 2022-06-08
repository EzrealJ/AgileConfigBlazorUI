namespace AgileConfig.UIApiClient.HttpModels
{
    public class SysLogVM
    {

        public int Id { get; set; }

        public string AppId { get; set; }

        public SysLogType LogType { get; set; }

        public DateTime? LogTime { get; set; }

        public string LogText { get; set; }

    }
}
