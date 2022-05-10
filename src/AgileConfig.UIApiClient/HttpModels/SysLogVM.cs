using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgileConfig.UIApiClient.HttpModels
{
    public class SysLogVM
    {

        public int Id { get; set; }

        public string AppId { get; set; }

        public SysLogType LogType { get; set; }

        public DateTime? LogTime { get; set; }

        public string LogText { get; set; }

    }
}
