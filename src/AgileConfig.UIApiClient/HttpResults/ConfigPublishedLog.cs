using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgileConfig.UIApiClient.HttpResults
{
    public class ConfigPublishedLog
    {
        public int Key { get; set; }
        public List<PublishDetail> List { get; set; } = new(0);
        public PublishTimeline TimelineNode { get; set; } = new();
       
    }
}
