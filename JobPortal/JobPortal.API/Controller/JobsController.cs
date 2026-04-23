using JobPortal.Application.Command;
using JobPortal.Application.Handlers;
using JobPortal.Application.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {

        private readonly CreateJobHandler _createHandler;
        private readonly SearchJobHandler _searchHandler;

        public JobsController(CreateJobHandler createHandler , SearchJobHandler searchHandler)
        {
          _createHandler = createHandler;
            _searchHandler = searchHandler;
        }

        [HttpPost]

        public async Task<IActionResult>Create(CreateJobCommand command)
        {
            var id = await _createHandler.Handle(command);   // calls business logic ,handler does real work like chef in restaurant & command/query order slip
            return  Ok("Jobs created sucessfully");
        }

        [HttpGet("search")]

        public async Task<IActionResult> Search(string keyword)
        {
            var result = await _searchHandler.Handle(new SearchJobQuery
            {
                Keyword = keyword
            });
            return Ok(result);
        }


    }
}
