using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Infrastructure.Persistance;
using Server.API.Web.DTOs;

namespace Server.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IRegistrationService _userRepo)
        {
            _userRepo = _userRepo;
        }

        /// <summary>
        /// Returns username availability status.
        /// </summary>
        /// <returns>Information is given username is available.</returns>
        [HttpGet("check-username")]
        [ProducesResponseType(typeof(UsernameAvailabilityResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsernameAvailabilityResponse>> CheckUsername([FromQuery] string username, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(new { message = "Username is required." });

            var exists = await _userRepo.IsUserNameTakenAsync(username, cancellationToken);
            return Ok(new UsernameAvailabilityResponse { IsAvailable = !exists });
        }
    }
}
