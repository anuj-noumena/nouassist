using Volo.Abp.Domain.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories
{
    /// <summary>
    /// Repository for FundingRound aggregate
    /// </summary>
    public interface IFundingRoundRepository : IRepository<FundingRound, Guid>
    {
        /// <summary>
        /// Finds funding rounds matching the given specification
        /// </summary>
        Task<List<FundingRound>> GetListAsync(
            CapTableSpecification<FundingRound> specification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets funding rounds by cap table id
        /// </summary>
        Task<List<FundingRound>> GetByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets funding rounds by status
        /// </summary>
        Task<List<FundingRound>> GetByStatusAsync(
            FundingRoundStatus status,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the latest funding round for a cap table
        /// </summary>
        Task<FundingRound> GetLatestByCapTableIdAsync(
            Guid capTableId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets active funding rounds
        /// </summary>
        Task<List<FundingRound>> GetActiveFundingRoundsAsync(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds funding round by name
        /// </summary>
        Task<FundingRound> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
