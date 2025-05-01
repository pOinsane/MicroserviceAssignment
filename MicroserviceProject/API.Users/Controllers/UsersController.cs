using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using CORE.APP.Features;
using APP.Users.Features.User;

namespace API.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;

        public UsersController(ILogger<UsersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new UserQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UsersGet Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while getting users."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserCreateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("UsersPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"UsersPost Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while creating user."));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put(int id, [FromBody] UserUpdateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.Id = id;
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("UsersPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"UsersPut Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while updating user."));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new UserDeleteCommand { Id = id });
                if (response.IsSuccessful)
                    return Ok(response);

                ModelState.AddModelError("UsersDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"UsersDelete Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while deleting user."));
            }
        }
    }
}
