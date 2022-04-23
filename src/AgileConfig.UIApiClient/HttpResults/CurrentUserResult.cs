using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{

    public class CurrentUserResult
    {
        public Currentuser CurrentUser { get; set; }
    }

    public class Currentuser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string[] CurrentAuthority { get; set; }
        public string[] CurrentFunctions { get; set; }
    }


}
