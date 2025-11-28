using Server.API.Domain.Entities;

namespace Server.API.Application.Abstractions.Persistance
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user, CancellationToken cancellationToken);

        Task<bool> IsUserNameTakenAsync(string userName, CancellationToken cancellationToken);
    }
}
