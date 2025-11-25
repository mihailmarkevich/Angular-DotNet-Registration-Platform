using Server.API.Models;

namespace Server.API.Services
{
    public interface IIndustryService
    {
        Task<IReadOnlyList<Industry>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
