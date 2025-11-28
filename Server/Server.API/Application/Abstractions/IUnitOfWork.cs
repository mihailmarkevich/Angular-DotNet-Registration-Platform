using Server.API.Domain.Entities;

namespace Server.API.Application.Abstractions
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(
            Func<CancellationToken, Task> action,
            CancellationToken cancellationToken);

    }
}
