using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Application.Abstractions;
using Server.API.Web.DTOs;
using Server.API.Web.Mappings;

namespace Server.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService, ILogger<RegistrationController> logger)
        {
            _registrationService = registrationService;
            _logger = logger;
        }

        /// <summary>
        /// Registers the user
        /// </summary>
        /// <returns>The information of the registered user.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(RegistrationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationResponseDto>> Register(
            [FromBody] RegistrationRequestDto request, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = request.ToCommand();
                var result = await _registrationService.RegisterAsync(command, cancellationToken);
                var response = result.ToResponseDto();

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error during registration.");
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business rule violation during registration.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration.");
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Unexpected error during registration",
                    detail: "An unexpected error occurred.");
            }
        }
    }
}
