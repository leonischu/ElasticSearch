using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Domain.Models
{
    public class Job
    {
        public int id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; } 

        public string Company { get; set; }

        public decimal Salary { get; set; } 

    }
}
