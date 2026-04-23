using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Messaging
{
    public class KafkaSetting
    {
        public string BootstrapServers { get; set; } = "localhost:9092";
        public string Topic { get; set; } = "job-updated";
        public string GroupId { get; set; } = "job-group";
    }
}
