using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Events
{


    public class DebeziumMessage<T>
    {
        public DebeziumPayload<T> Payload { get; set; }
    }

    public class DebeziumPayload<T>
    {
        public T Before { get; set; }
        public T After { get; set; }
        public string Op { get; set; } // c = create, u = update, d = delete
    }
}
