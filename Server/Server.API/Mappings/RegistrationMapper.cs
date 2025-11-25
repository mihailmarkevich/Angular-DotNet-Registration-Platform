using Server.API.Application;
using Server.API.DTOs;

namespace Server.API.Mappings
{
    public static class RegistrationMapper
    {
        public static RegistrationCommand ToCommand(this RegistrationRequestDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new RegistrationCommand
            {
                CompanyName = dto.CompanyName,
                IndustryId = dto.IndustryId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email,
                Password = dto.Password
            };
        }

        public static RegistrationResponseDto ToResponseDto(this RegistrationResult result)
        {
            if (result is null) throw new ArgumentNullException(nameof(result));

            return new RegistrationResponseDto
            {
                CompanyId = result.CompanyId,
                UserId = result.UserId
            };
        }
    }

}
