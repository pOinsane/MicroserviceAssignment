using Microsoft.AspNetCore.Mvc;
using MediatR;
using APP.Books.Features.Book;
using CORE.APP.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Books.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new BookQueryRequest());
            return Ok(await result.ToListAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new BookQueryRequest());
            var book = await result.SingleOrDefaultAsync(x => x.Id == id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] BookCreateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put([FromBody] BookUpdateRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new BookDeleteRequest { Id = id });
            return Ok(response);
        }
    }
}
