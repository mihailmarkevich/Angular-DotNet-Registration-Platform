using Server.API.Domain.Entities;

namespace Server.API.Application.Abstractions
{
    public interface IIndustryService
    {
        Task<IReadOnlyList<Industry>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
