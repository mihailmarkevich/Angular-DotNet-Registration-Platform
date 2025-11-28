using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Domain.Entities;
using Server.API.Infrastructure.Persistance;

namespace Server.API.Infrastructure.Persistance.Repositories
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly AppDbContext _dbContext;

        public IndustryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Industry>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Industries
                .AsNoTracking()           
                .OrderBy(i => i.Name)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> IndustryExistsAsync(int industryId, CancellationToken cancellationToken) =>
            _dbContext.Industries.AnyAsync(i => i.Id == industryId, cancellationToken);

    }

}
