using Server.API.Application;

namespace Server.API.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(RegistrationCommand command, CancellationToken cancellationToken = default);
    }
}
