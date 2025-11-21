using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Exceptions;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IVersionedRequirementHistoryRepository
    /// </summary>
    public class EfCoreVersionedRequirementHistoryRepository :
        EfCoreRepository<IVersionControlDbContext, VersionedRequirementHistory, Guid>,
        IVersionedRequirementHistoryRepository
    {
        public EfCoreVersionedRequirementHistoryRepository(
            IDbContextProvider<IVersionControlDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets the version history for a specific requirement
        /// </summary>
        public async Task<VersionedRequirementHistory> GetByRequirementIdAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var history = await dbContext.VersionedRequirementHistories
                .Include(h => h.VersionRecords)
                .Where(h => h.RequirementId == requirementId)
                .FirstOrDefaultAsync(cancellationToken);

            if (history == null)
            {
                throw new RequirementVersionNotFoundException(requirementId, "any");
            }

            return history;
        }

        /// <summary>
        /// Checks if a version history exists for a specific requirement
        /// </summary>
        public async Task<bool> ExistsByRequirementIdAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.VersionedRequirementHistories
                .AnyAsync(h => h.RequirementId == requirementId, cancellationToken);
        }
    }
}
