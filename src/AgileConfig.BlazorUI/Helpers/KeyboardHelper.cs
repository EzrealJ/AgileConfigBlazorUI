using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;

namespace AgileConfig.BlazorUI.Helpers
{
    public static class KeyboardHelper
    {
        public static async Task OnEnterAsync(KeyboardEventArgs args, Func<Task> action)
        {
            if(args.Key == "Enter")
            {
               await action();
            }
        }
    }
}
