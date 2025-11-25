using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.Data;
using Server.API.DTOs;

namespace Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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

            var exists = await _dbContext.Users.AnyAsync(u => u.UserName == username, cancellationToken);
            return Ok(new UsernameAvailabilityResponse { IsAvailable = !exists });
        }
    }
}
