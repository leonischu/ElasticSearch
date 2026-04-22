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
            await _client.IndexDocumentAsync(entity);
        }

        public async Task<IEnumerable<T>> SearchAsync(string keyword)
        {
            var response = await _client.SearchAsync<T>(s => s
                .Query(q => q
                    .QueryString(d => d.Query($"*{keyword}*"))
                ));

            return response.Documents;
        }
    }
}
