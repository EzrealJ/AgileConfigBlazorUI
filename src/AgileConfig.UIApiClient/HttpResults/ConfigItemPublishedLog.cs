namespace AgileConfig.UIApiClient.HttpResults
{
    public class PublishDetail
    {
        public string Id { get; set; }

        public string AppId { get; set; }
        public int Version { get; set; }

        public string PublishTimelineId { get; set; }

        public string ConfigId { get; set; }

        public string Group { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public EditStatus EditStatus { get; set; }
        public string Env { get; set; }
    }

    public class PublishTimeline
    {
        public string Id { get; set; }

        public string AppId { get; set; }

        public DateTime? PublishTime { get; set; }

        public string PublishUserId { get; set; }

        public string PublishUserName { get; set; }

        public int Version { get; set; }

        public string Log { get; set; }

        public string Env { get; set; }
    }
    public class ConfigItemPublishedLog
    {
        public PublishTimeline TimelineNode { get; set; }
        public PublishDetail Config { get; set; }


    }
}
