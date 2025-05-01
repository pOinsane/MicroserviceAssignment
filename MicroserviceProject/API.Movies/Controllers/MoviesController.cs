using APP.Movies.Features.Movie;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMediator mediator, ILogger<MoviesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new MovieQueryRequest());
            var list = await result.ToListAsync();
            if (!list.Any())
                return NoContent();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new MovieQueryRequest());
            var item = await result.SingleOrDefaultAsync(m => m.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] MovieCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid data"));

            var response = await _mediator.Send(request);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] MovieUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid data"));

            var response = await _mediator.Send(request);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new MovieDeleteRequest { Id = id });
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }
    }
}
