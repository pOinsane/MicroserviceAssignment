using Microsoft.AspNetCore.Mvc;
using MediatR;
using APP.Books.Features.Author;
using CORE.APP.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Books.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new AuthorQueryRequest());
            return Ok(await result.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new AuthorQueryRequest());
            var author = await result.SingleOrDefaultAsync(x => x.Id == id);
            return author == null ? NotFound() : Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] AuthorCreateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] AuthorUpdateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new AuthorDeleteRequest { Id = id });
            return Ok(response);
        }
    }
}
