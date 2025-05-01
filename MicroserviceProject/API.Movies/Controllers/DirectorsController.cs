using APP.Movies.Features.Director;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DirectorsController> _logger;

        public DirectorsController(IMediator mediator, ILogger<DirectorsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new DirectorQueryRequest());
            var list = await result.ToListAsync();
            if (!list.Any())
                return NoContent();
            return Ok(list);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new DirectorQueryRequest());
            var item = await result.SingleOrDefaultAsync(d => d.Id == id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] DirectorCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid data"));

            var response = await _mediator.Send(request);
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] DirectorUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid data"));

            var response = await _mediator.Send(request);
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DirectorDeleteRequest { Id = id });
            if (!response.IsSuccessful)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
