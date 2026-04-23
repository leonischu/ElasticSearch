using JobPortal.Application.Interfaces;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Data.SqlClient.Internal.SqlClientEventSource;

namespace JobPortal.Infrastructure.Services
{
    public class ElasticService<T> : IElasticService<T> where T : class
    {
        private readonly ElasticClient _client;

        public ElasticService(ElasticClient client)
        {
            _client = client;
        }

        public async Task IndexAsync(T entity)
        {
            var response = await _client.IndexAsync(entity, i => i
                .Index("jobs")   //  FORCE index name , stores data in elastic search 
            );

            Console.WriteLine(response.IsValid);
            Console.WriteLine(response.DebugInformation);
        }

        public async Task<IEnumerable<T>> SearchAsync(string keyword)  // esle title description and company ma keyword search garcha   
        {
            var response = await _client.SearchAsync<T>(s => s
                .Index("jobs") 
                .Query(q => q
                    .MultiMatch(m => m
                        .Query(keyword)
                        .Fields(f => f
                            .Field("title")
                            .Field("description")
                            .Field("company")
                        )
                    )
                )
            );

            Console.WriteLine(response.IsValid);
            Console.WriteLine(response.DebugInformation);

            return response.Documents;
        }
    }
}
