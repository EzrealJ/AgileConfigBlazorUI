namespace AgileConfig.UIApiClient.HttpResults
{
    public class ServerNodeResult
    {
        public bool Success { get; set; }
        public IEnumerable<ServerNodeVM> Data { get; set; }
    }


}
