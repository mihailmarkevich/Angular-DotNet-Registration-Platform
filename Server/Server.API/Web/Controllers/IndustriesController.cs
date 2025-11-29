using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Web.DTOs;

namespace Server.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndustriesController : ControllerBase
    {
        private readonly IIndustryRepository _industryRepo;

        public IndustriesController(IIndustryRepository industryService)
        {
            _industryRepo = industryService;
        }

        /// <summary>
        /// Returns list of available industries.
        /// </summary>
        /// <returns>List of industries.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IndustryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IndustryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var industries = await _industryRepo.GetAllAsync(cancellationToken);

            var result = industries.ToDtoList();

            return Ok(industries);
        }
    }
}
