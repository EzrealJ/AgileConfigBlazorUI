using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class ServerNodeResult
    {
        public bool Success { get; set; }
        public IEnumerable<ServerNodeVM> Data { get; set; }
    }


}
