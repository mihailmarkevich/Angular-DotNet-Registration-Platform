using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions;
using Server.API.Domain.Entities;
using Server.API.Infrastructure.Persistance;

namespace Server.API.Infrastructure.Services
{
    public class IndustryService : IIndustryService
    {
        private readonly AppDbContext _dbContext;

        public IndustryService(AppDbContext dbContext)
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
    }

}
