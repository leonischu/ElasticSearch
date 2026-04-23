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
        private readonly UpdateJobHandler _updateHandler;

        public JobsController(CreateJobHandler createHandler , SearchJobHandler searchHandler,UpdateJobHandler updateHandler)
        {
            _createHandler = createHandler;
            _searchHandler = searchHandler;
            _updateHandler = updateHandler;
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


        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, UpdateJobCommand command)
        {



            if (id != command.Id) // User sending wrong id in the url
                return BadRequest("Id mismatch");

            var result = await _updateHandler.Handle(command);

            if (!result)
                return NotFound("Job not found");
            return Ok("Job Updated Sucessfully");
        }







    }
}
