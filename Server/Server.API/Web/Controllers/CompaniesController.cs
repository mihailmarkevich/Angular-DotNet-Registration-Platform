using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Web.DTOs;
using Server.API.Web.Mappings;

namespace Server.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyService)
        {
            _companyRepo = companyService;
        }

        /// <summary>
        /// Searches existing companies by name (prefix) and optional industry.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<CompanySuggestionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CompanySuggestionDto>>> Search(
            [FromQuery] string query,
            [FromQuery] int? industryId,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
                return Ok(Array.Empty<CompanySuggestionDto>());

            var companies = await _companyRepo
                .SearchCompaniesAsync(query.Trim(), industryId, cancellationToken);

            var result = companies.ToSuggestionDtoList();

            return Ok(result);
        }

    }
}
