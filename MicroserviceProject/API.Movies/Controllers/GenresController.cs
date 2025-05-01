using APP.Movies.Features.Genre;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GenresController> _logger;

        public GenresController(IMediator mediator, ILogger<GenresController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GenreQueryRequest());
            var list = await result.ToListAsync();
            if (!list.Any())
                return NoContent();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GenreQueryRequest());
            var item = await result.SingleOrDefaultAsync(g => g.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] GenreCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid data"));

            var response = await _mediator.Send(request);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] GenreUpdateRequest request)
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
            var response = await _mediator.Send(new GenreDeleteRequest { Id = id });
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }
    }
}
