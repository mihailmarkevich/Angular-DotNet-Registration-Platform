using Server.API.Domain.Entities;
using Server.API.Web.DTOs;

namespace Server.API.Web.Mappings
{
    public static class IndustryMappings
    {
        public static IndustryDto ToDto(this Industry entity)
        {
            if (entity is null) throw new ArgumentNullException(nameof(entity));

            return new IndustryDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static IEnumerable<IndustryDto> ToDtoList(this IEnumerable<Industry> entities)
        {
            if (entities is null) throw new ArgumentNullException(nameof(entities));

            return entities.Select(e => e.ToDto());
        }
    }

}