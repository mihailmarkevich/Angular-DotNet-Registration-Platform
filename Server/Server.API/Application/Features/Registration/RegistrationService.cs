using Server.API.Application.Abstractions;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Domain.Entities;

namespace Server.API.Application.Features.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IIndustryRepository _industries;
        private readonly ICompanyRepository _companies;
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegistrationService> _logger;

        public RegistrationService(
            IIndustryRepository industries,
            ICompanyRepository companies,
            IUserRepository users,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            ILogger<RegistrationService> logger)
        {
            _industries = industries;
            _companies = companies;
            _users = users;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
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

            if (!await _industries.IndustryExistsAsync(command.IndustryId, cancellationToken))
                throw new InvalidOperationException("Selected industry does not exist.");

            if (await _users.IsUserNameTakenAsync(userName, cancellationToken))
                throw new InvalidOperationException("Username is already taken.");

            Company? company = null;
            User? user = null;

            try
            {
                await _unitOfWork.ExecuteInTransactionAsync(async ct =>
                {
                    company = await _companies.FindCompanyAsync(
                        companyName,
                        command.IndustryId,
                        ct);

                    if (company is null)
                    {
                        company = new Company
                        {
                            Name = companyName,
                            IndustryId = command.IndustryId,
                            CreatedAt = DateTime.UtcNow
                        };

                        await _companies.AddCompanyAsync(company, ct);
                    }

                    _passwordHasher.HashPassword(command.Password, out var hash, out var salt);
                    var now = DateTime.UtcNow;

                    user = new User
                    {
                        Company = company, // EF will set CompanyId internally
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

                    await _users.AddUserAsync(user, ct);
                }, cancellationToken);

                return new RegistrationResult
                {
                    CompanyId = company!.Id,
                    UserId = user!.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration.");
                throw;
            }
        }

    }
}
