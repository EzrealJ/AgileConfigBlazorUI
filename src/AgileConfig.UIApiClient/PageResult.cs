namespace AgileConfig.UIApiClient
{
    public class PageResult<T>
    {
        public int Current { get; set; }
        public int PageSize { get; set; }
        public bool Success { get; set; }
        public int Total { get; set; }
        public List<T> Data { get; set; } = new();

    }
}
