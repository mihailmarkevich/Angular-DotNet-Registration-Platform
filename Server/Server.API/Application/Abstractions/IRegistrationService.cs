using Server.API.Application.Features.Registration;

namespace Server.API.Application.Abstractions
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(RegistrationCommand command, CancellationToken cancellationToken = default);
    }
}
