using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.DTO
{
    public class JobDto
    {
        public int Id { get; set; }  
        public string Title { get; set; }

        public string Company {  get; set; }    

        public decimal Salary { get; set; } 
    }
}
