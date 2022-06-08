namespace AgileConfig.UIApiClient.HttpResults
{
    public class ConfigPublishedLog
    {
        public int Key { get; set; }
        public List<PublishDetail> List { get; set; } = new(0);
        public PublishTimeline TimelineNode { get; set; } = new();

    }
}
