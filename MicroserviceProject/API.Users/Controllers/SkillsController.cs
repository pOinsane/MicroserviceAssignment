using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using CORE.APP.Features;
using APP.Users.Features.Skill;

namespace API.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly IMediator _mediator;

        public SkillsController(ILogger<SkillsController> logger, IMediator mediator)
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
                var response = await _mediator.Send(new SkillQuery());
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SkillsGet Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while getting skills."));
            }
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Post([FromBody] SkillCreateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("SkillsPost", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"SkillsPost Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while creating skill."));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Put(int id, [FromBody] SkillUpdateCommand request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    request.Id = id;
                    var response = await _mediator.Send(request);
                    if (response.IsSuccessful)
                        return Ok(response);

                    ModelState.AddModelError("SkillsPut", response.Message);
                }
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"SkillsPut Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while updating skill."));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _mediator.Send(new SkillDeleteCommand { Id = id });
                if (response.IsSuccessful)
                    return Ok(response);

                ModelState.AddModelError("SkillsDelete", response.Message);
                return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
            }
            catch (Exception ex)
            {
                _logger.LogError($"SkillsDelete Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An error occurred while deleting skill."));
            }
        }
    }
}
