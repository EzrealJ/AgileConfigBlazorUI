using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class LoginResult:ApiResult
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public List<string> CurrentAuthority { get; set; }
        public List<string> CurrentFunctions { get; set; }
    }
}
