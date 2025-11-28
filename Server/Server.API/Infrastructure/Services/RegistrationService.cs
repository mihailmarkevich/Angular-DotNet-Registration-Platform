using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions;
using Server.API.Application.Features.Registration;
using Server.API.Domain.Entities;
using Server.API.Infrastructure.Persistance;

namespace Server.API.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(
            AppDbContext dbContext,
            IPasswordHasher passwordHasher,
            ILogger<RegistrationService> logger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<RegistrationResult> RegisterAsync(
            RegistrationCommand command,
            CancellationToken cancellationToken = default)
        {
            var companyName = command.CompanyName.Trim();
            var firstName = command.FirstName.Trim();
            var lastName = command.LastName.Trim();
            var userName = command.UserName.Trim();
            var email = string.IsNullOrWhiteSpace(command.Email)
                ? null
                : command.Email.Trim();

            var industryExists = await _dbContext.Industries
                .AnyAsync(i => i.Id == command.IndustryId, cancellationToken);
            if (!industryExists)
                throw new InvalidOperationException("Selected industry does not exist.");

            var usernameTaken = await _dbContext.Users
                .AnyAsync(u => u.UserName == userName, cancellationToken);
            if (usernameTaken)
                throw new InvalidOperationException("Username is already taken.");

            using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var company = await _dbContext.Companies
                    .FirstOrDefaultAsync(
                        c => c.Name == companyName && c.IndustryId == command.IndustryId,
                        cancellationToken);

                if (company is null)
                {
                    company = new Company
                    {
                        Name = companyName,
                        IndustryId = command.IndustryId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _dbContext.Companies.Add(company);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

                _passwordHasher.HashPassword(command.Password, out var hash, out var salt);

                var now = DateTime.UtcNow;

                var user = new User
                {
                    CompanyId = company.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Email = email,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    CreatedAt = now,
                    TermsAcceptedAt = now,
                    PrivacyAcceptedAt = now
                };

                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new RegistrationResult
                {
                    CompanyId = company.Id,
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration. Rolling back transaction.");
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

    }

}
