using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions;
using Server.API.Domain.Entities;

namespace Server.API.Infrastructure.Persistance
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public EfUnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken)
        {
            await using var transaction =
                await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                await action(cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

    }
}
