namespace AgileConfig.UIApiClient.HttpResults
{
    public class AppListVM : AppVM
    {
        public DateTime? UpdateTime { get; set; }

        public List<AppListVM> Children { get; set; } = new();

        public bool IsGroupTitle => Children?.Any() ?? false;

        public void ResetGroupTitle()
        {
            if (!IsGroupTitle)
            {
                return;
            }
            Name = $"{Group} Group";
            Id = Name;
            InheritancedAppNames = new List<string>();
            InheritancedApps = new List<string>();
        }
    }
}
