using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class TokenResult:ApiResult
    {
        public string Token { get; set; }
        public string Type { get; set; }
    }
}
