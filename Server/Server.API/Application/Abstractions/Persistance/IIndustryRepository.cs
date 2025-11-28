using Server.API.Domain.Entities;

namespace Server.API.Application.Abstractions.Persistance
{
    public interface IIndustryRepository
    {
        Task<IReadOnlyList<Industry>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<bool> IndustryExistsAsync(int industryId, CancellationToken cancellationToken);
    }
}
