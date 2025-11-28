using Server.API.Domain.Entities;

namespace Server.API.Application.Abstractions.Persistance
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Returns a list of companies whose name starts with the given query,
        /// optionally filtered by industry.
        /// </summary>
        Task<IReadOnlyList<Company>> SearchCompaniesAsync(
            string query,
            int? industryId,
            CancellationToken cancellationToken = default);

        Task<Company?> FindCompanyAsync(
            string companyName,
            int industryId,
            CancellationToken cancellationToken);

        Task AddCompanyAsync(Company company, CancellationToken cancellationToken);
    }

}
