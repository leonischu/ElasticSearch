
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

        public async Task<IEnumerable<T>> GetAllDocuments()
        {
            var searchResponse = await _elasticClient.SearchAsync<T>(s => s.MatchAll().Size(1000));
            return searchResponse.Documents;
        }

        public async Task<T> GetDocumentAsync(int id)
        {
            var response = await _elasticClient.GetAsync(new DocumentPath<T>(id));
            return response.Source;
        }

        public async Task<string> UpdateDocumentAsync(T document)
        {
            var response = await _elasticClient.UpdateAsync(new DocumentPath<T>(document), u => u
            .Doc(document)
            .RetryOnConflict(3));
            return response.IsValid ? "Document updated sucessfully" : "Failed to update document";
        }
    }
}
