using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class AppListVM:AppVM
    {
        public DateTime? UpdateTime { get; set; }

        public List<AppListVM> Children { get; set; } = new();
    }
}
