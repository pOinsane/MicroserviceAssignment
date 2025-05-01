using Microsoft.AspNetCore.Mvc;
using MediatR;
using APP.Books.Features.Genre;
using CORE.APP.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Books.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenresController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GenreQueryRequest());
            return Ok(await result.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GenreQueryRequest());
            var genre = await result.SingleOrDefaultAsync(x => x.Id == id);
            return genre == null ? NotFound() : Ok(genre);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] GenreCreateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] GenreUpdateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new GenreDeleteRequest { Id = id });
            return Ok(response);
        }
    }
}
