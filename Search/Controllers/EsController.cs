using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Search.Models;
using Search.Services;

namespace Search.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EsController : ControllerBase
    {
        private readonly IElasticSearchService<MyDocument> _elasticSearchService;

        public EsController(IElasticSearchService<MyDocument> elasticSearchService)
        {
            
            _elasticSearchService = elasticSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDocuments()
        {
            var response = await _elasticSearchService.GetAllDocuments();   
            return Ok(response);
        }

        [HttpPost]
        
        public async Task<IActionResult> CreateDocument([FromBody] MyDocument document)
        {
            var result = await _elasticSearchService.CreateDocumentAsync(document);
            return Ok(result);
        }

        [HttpGet("read/{id}")]
       public async Task<IActionResult> GetDocument(int id)
        {
             var document = await _elasticSearchService.GetDocumentAsync(id);

            if(document == null) 
                return NotFound();
            return Ok(document);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocument([FromBody] MyDocument document)
        {
            var result = await _elasticSearchService.UpdateDocumentAsync(document);
            return Ok(result);
        
        }

        [HttpDelete ("delete/{id}")]

        public async Task<IActionResult>DeleteDocument(int id)
        {
            var result = await _elasticSearchService.DeleteDocumentAsync(id);
            return Ok(result);
        }

    }
}
