using JobPortal.Application.Command;
using JobPortal.Application.Interfaces;
using JobPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Handlers
{
    public class CreateJobHandler
    {
        private readonly IJobRepository _repo;
        private readonly IElasticService<Job> _elastic;

        public CreateJobHandler(IJobRepository repo, IElasticService<Job> elastic)
        {
            _repo = repo;
            _elastic = elastic;
        }

        public async Task<int>Handle(CreateJobCommand command)
        {
            var job = new Job
            {
                Title = command.Title,
                Description = command.Description,
                Company = command.Company,
                Salary = command.Salary
            };
            var id = await _repo.CreateAsync(job);
            return id;
        }
    }
}
