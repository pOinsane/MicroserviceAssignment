using APP.Auth.Features.Auth;
using APP.Auth.Features.Login;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid input"));

            var response = await _mediator.Send(request);
            if (response.IsSuccessful)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new CommandResponse(false, "Invalid input"));

            var response = await _mediator.Send(request);

            if (response.IsSuccessful)
            {
                var token = response.Message;
                return Ok(new { token });
            }

            return Unauthorized(response);
        }


    }
}
