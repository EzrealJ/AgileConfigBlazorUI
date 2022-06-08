namespace AgileConfig.UIApiClient.HttpResults
{
    public class AppListVM : AppVM
    {
        public DateTime? UpdateTime { get; set; }

        public List<AppListVM> Children { get; set; } = new();
    }
}
