using Microsoft.EntityFrameworkCore;
using Server.API.Application.Abstractions.Persistance;
using Server.API.Domain.Entities;
using Server.API.Infrastructure.Persistance;

namespace Server.API.Infrastructure.Persistance.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _dbContext;

        public CompanyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Company>> SearchCompaniesAsync(
            string query,
            int? industryId,
            CancellationToken cancellationToken = default)
        {
            var q = query.Trim();

            var companiesQuery = _dbContext.Companies
                .AsNoTracking()
                .Where(c => c.Name.StartsWith(q));

            if (industryId.HasValue && industryId.Value > 0)
            {
                companiesQuery = companiesQuery.Where(c => c.IndustryId == industryId.Value);
            }

            var companies = await companiesQuery
                .OrderBy(c => c.Name)
                .Take(5)
                .ToListAsync(cancellationToken);

            return companies;
        }

        public Task<Company?> FindCompanyAsync(
           string companyName,
           int industryId,
           CancellationToken cancellationToken) =>
           _dbContext.Companies
               .FirstOrDefaultAsync(
                   c => c.Name == companyName && c.IndustryId == industryId,
                   cancellationToken);

        public Task AddCompanyAsync(Company company, CancellationToken cancellationToken)
        {
            _dbContext.Companies.Add(company);
            return Task.CompletedTask;
        }

    }

}
