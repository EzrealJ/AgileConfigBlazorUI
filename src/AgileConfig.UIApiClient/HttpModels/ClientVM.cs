namespace AgileConfig.UIApiClient.HttpModels
{
    public class ClientVM
    {
        public string Id { get; set; } = string.Empty;

        public string AppId { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Ip { get; set; } = string.Empty;

        public string Env { get; set; } = string.Empty;

        public DateTime LastHeartbeatTime { get; set; }
    }
}
