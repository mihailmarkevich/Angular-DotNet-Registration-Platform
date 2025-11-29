using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Web.DTOs;
using Server.API.Web.Mappings;

namespace Server.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndustriesController : ControllerBase
    {
        private readonly IIndustryRepository _industryRepo;
        private readonly ILogger<IndustriesController> _logger;

        public IndustriesController(IIndustryRepository industryService, ILogger<IndustriesController> logger)
        {
            _industryRepo = industryService;
            _logger = logger;
        }

        /// <summary>
        /// Returns list of available industries.
        /// </summary>
        /// <returns>List of industries.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IndustryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IndustryDto>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                var industries = await _industryRepo.GetAllAsync(cancellationToken);

                var result = industries.ToDtoList();

                return Ok(result);
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
