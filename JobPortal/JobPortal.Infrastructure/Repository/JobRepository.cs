using Dapper;
using JobPortal.Application.Interfaces;
using JobPortal.Domain.Models;
using JobPortal.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly DapperContext _context;

        public JobRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Job job)
        {
            var sql = @"INSERT INTO Jobs(Title,Description,Company,Salary) 
                        VALUES (@Title,@Description,@Company,@Salary);   
                        SELECT CAST(SCOPE_IDENTITY() as int);";
            
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, job);
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            var sql = "SELECT * FROM Jobs";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Job>(sql);
        }

        public async Task<Job> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Jobs WHERE Id = @Id";

            using var connection = _context.CreateConnection();

            return await connection.QueryFirstOrDefaultAsync<Job>(sql, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Job job)
        {
            var query = @"UPDATE Jobs SET  Title = @Title,
                        Description=@Description,
                        Company = @Company,
                        Salary=@Salary
                        WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return  await connection.ExecuteAsync(query, job) > 0;
        }
    }
}
