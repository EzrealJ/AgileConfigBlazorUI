using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class HomeSysResult
    {
        public string AppVer { get; set; }
        public bool PasswordInited { get; set; }

        public string[] EnvList { get; set; }
        
    }
}
