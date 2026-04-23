using JobPortal.Application.Command;
using JobPortal.Application.Events;
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
        private readonly IKafkaProducer _producer;


        public UpdateJobHandler(IJobRepository repo, IElasticService<Job> elastic,IKafkaProducer producer)
        {
            _repo = repo;
            _elastic = elastic;
            _producer = producer;
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
            //await _elastic.IndexAsync(existing); // overwrites document 

            //Publish event i.e sends event to Kafka 
            await _producer.ProduceAsync(new JobUpdatedEvent
            {
                Id = existing.Id,
                Title = existing.Title,
                Description = existing.Description,
                Company =existing.Company,
                Salary = existing.Salary
            });


            return true;

        }



    }
}
