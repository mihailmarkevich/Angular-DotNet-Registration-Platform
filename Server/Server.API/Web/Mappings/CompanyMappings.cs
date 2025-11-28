using Server.API.Domain.Entities;
using Server.API.Web.DTOs;

namespace Server.API.Web.Mappings
{
    public static class CompanyMappings
    {
        public static CompanySuggestionDto ToSuggestionDto(this Company entity)
        {
            return new CompanySuggestionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IndustryId = entity.IndustryId
            };
        }

        public static IEnumerable<CompanySuggestionDto> ToSuggestionDtoList(
            this IEnumerable<Company> entities)
        {
            return entities.Select(e => e.ToSuggestionDto()).ToList();
        }
    }
}
