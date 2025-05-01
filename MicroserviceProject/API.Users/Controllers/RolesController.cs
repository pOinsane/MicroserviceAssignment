using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CORE.APP.Features;
using APP.Users.Features.Role;

namespace API.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IMediator _mediator;

        public RolesController(ILogger<RolesController> logger, IMediator mediator)
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
                var response = await _mediator.Send(new RoleQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"RolesGet Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while getting roles."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RoleCreateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("RolesPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"RolesPost Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while creating role."));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put(int id, [FromBody] RoleUpdateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.Id = id;
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("RolesPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"RolesPut Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while updating role."));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new RoleDeleteCommand { Id = id });
                if (response.IsSuccessful)
                    return Ok(response);

                ModelState.AddModelError("RolesDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"RolesDelete Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while deleting role."));
            }
        }
    }
}
