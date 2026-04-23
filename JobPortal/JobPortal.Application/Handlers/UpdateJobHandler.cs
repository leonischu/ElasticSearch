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
    public class UpdateJobHandler
    {
        private readonly IJobRepository _repo;
        private readonly IElasticService<Job> _elastic;

        public UpdateJobHandler(IJobRepository repo, IElasticService<Job> elastic)
        {
            _repo = repo;
            _elastic = elastic;
        }

        public async Task<bool> Handle(UpdateJobCommand command)
        {


            //Get the existing repo

            var existing = await _repo.GetByIdAsync(command.Id);
            if (existing == null)
            {
                return false;
            }


            //Update values 

            existing.Title = command.Title;
            existing.Description = command.Description;
            existing.Company = command.Company;
            existing.Salary = command.Salary;

            //Updates in db 
            await _repo.UpdateAsync(existing);

            //updates in elastic search
            await _elastic.IndexAsync(existing); // overwrites document 
            return true;

        }



    }
}
