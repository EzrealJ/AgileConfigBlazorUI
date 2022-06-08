using System.Text.Json.Serialization;

namespace AgileConfig.UIApiClient.HttpResults
{

    public class RemoteNodeStatus
    {
        public ServerNodeVM? N { get; set; }

        [JsonPropertyName("server_status")]
        public ServerStatus ServerStatus { get; set; }
    }

    public class ServerStatus
    {
        public int ClientCount { get; set; }
        public ServerInfo[] Infos { get; set; }
    }

    public class ServerInfo
    {
        public string Id { get; set; }
        public string AppId { get; set; }
        public string Address { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Env { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
    }


}
