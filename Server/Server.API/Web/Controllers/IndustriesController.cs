using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions;
using Server.API.Web.DTOs;

namespace Server.API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndustriesController : ControllerBase
    {
        private readonly IIndustryService _industryService;

        public IndustriesController(IIndustryService industryService)
        {
            _industryService = industryService;
        }

        /// <summary>
        /// Returns list of available industries.
        /// </summary>
        /// <returns>List of industries.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IndustryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<IndustryDto>>> GetAll(CancellationToken cancellationToken)
        {
            var industries = await _industryService.GetAllAsync(cancellationToken);

            var result = industries.ToDtoList();

            return Ok(industries);
        }
    }
}
