using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Domain.Entities;

namespace Server.API.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> IsUserNameTakenAsync(string userName, CancellationToken cancellationToken) =>
            _dbContext.Users.AnyAsync(u => u.UserName == userName, cancellationToken);

        public Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            _dbContext.Users.Add(user);
            return Task.CompletedTask;
        }
    }
}
