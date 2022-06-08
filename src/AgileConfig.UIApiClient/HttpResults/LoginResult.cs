namespace AgileConfig.UIApiClient.HttpResults
{
    public class LoginResult : TokenResult
    {

        public List<string> CurrentAuthority { get; set; }
        public List<string> CurrentFunctions { get; set; }
    }
}
