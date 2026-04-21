
using Nest;

namespace Search.Services
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {

        private readonly ElasticClient _elasticClient;

        public ElasticSearchService(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }



        public async Task<string> CreateDocumentAsync(T document)
        {
           var response =  await _elasticClient.IndexDocumentAsync(document);
            return response.IsValid ? "Document created sucessfully" : "Failed to create document";
        }

        public async  Task<string> DeleteDocumentAsync(int id)
        {
            var response = await _elasticClient.DeleteAsync(new DocumentPath<T>(id));
            return response.IsValid ? "Document deleted sucessfully" : "Failed to delete document";
        }

        public Task<IEnumerable<T>> GetAllDocuments()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetDocumentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateDocumentAsync(T document)
        {
            throw new NotImplementedException();
        }
    }
}
