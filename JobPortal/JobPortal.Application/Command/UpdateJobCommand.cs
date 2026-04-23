using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Command
{
    public class UpdateJobCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Title { get; set; }   

        public string Description { get; set; }

        public string Company {  get; set; }

        public decimal Salary { get; set; }
    }
}
