using JobPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Interfaces
{
    public interface IJobRepository
    {
        Task<int> CreateAsync(Job job);

        Task<IEnumerable<Job>> GetAllAsync();  
        
        Task <bool>UpdateAsync(Job job);
        Task<Job> GetByIdAsync(int id); 
    }
}
