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
    public class CreateJobHandler  //Handlers vaneko workers 
    {
        private readonly IJobRepository _repo; //Talks to sql
        private readonly IElasticService<Job> _elastic;  //Talks to elastic search

        public CreateJobHandler(IJobRepository repo, IElasticService<Job> elastic)
        {
            _repo = repo;
            _elastic = elastic;  //
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
            var id = await _repo.CreateAsync(job);    // creates job object and saves in sql 

            job.Id = id;
            await _elastic.IndexAsync(job);   // send to elastic search

            return id;
        }
    }
}
