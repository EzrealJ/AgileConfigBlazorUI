using System.Collections.Generic;

namespace AgileConfig.BlazorUI.Helpers
{
    public class LayoutHelper
    {
        private static readonly Dictionary<string, int> _gutterX = new()
        {
            ["xs"] = 8,
            ["sm"] = 16,
            ["md"] = 24,
            ["lg"] = 32,
            ["xl"] = 48,
            ["xxl"] = 64
        };

        private static readonly int _gutterY = 24;
        internal static (Dictionary<string, int> X, int Y) Gutter => (_gutterX, _gutterY);
    }
}
