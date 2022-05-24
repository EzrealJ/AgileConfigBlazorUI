using System.Threading.Tasks;

namespace AgileConfig.BlazorUI.Extensions
{
    public static class TaskExtensions
    {
        public static TResult GetResultSync<TResult>(this Task<TResult> task)
            => task.ConfigureAwait(false).GetAwaiter().GetResult();
        public static TResult GetResultSync<TResult>(this ValueTask<TResult> task)
          => task.ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
