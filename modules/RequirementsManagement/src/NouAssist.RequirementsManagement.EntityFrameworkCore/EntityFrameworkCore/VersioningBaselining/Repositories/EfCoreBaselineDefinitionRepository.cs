using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IBaselineDefinitionRepository
    /// </summary>
    public class EfCoreBaselineDefinitionRepository :
        EfCoreRepository<IVersionControlDbContext, BaselineDefinition, Guid>,
        IBaselineDefinitionRepository
    {
        public EfCoreBaselineDefinitionRepository(
            IDbContextProvider<IVersionControlDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets a baseline by ID with its items
        /// </summary>
        public override async Task<BaselineDefinition> GetAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.BaselineDefinitions.AsQueryable();
            
            if (includeDetails)
            {
                query = query.Include(b => b.Items);
            }
            
            var baseline = await query
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (baseline == null)
            {
                throw new EntityNotFoundException(typeof(BaselineDefinition), id);
            }
            
            return baseline;
        }

        /// <summary>
        /// Checks if a baseline with the given name already exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.BaselineDefinitions.AsQueryable();
            
            if (excludeId.HasValue)
            {
                query = query.Where(b => b.Id != excludeId.Value);
            }
            
            return await query
                .AnyAsync(b => b.Name == name, cancellationToken);
        }

        /// <summary>
        /// Gets a list of baselines by status
        /// </summary>
        public async Task<List<BaselineDefinition>> GetListByStatusAsync(
            BaselineStatus status,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.BaselineDefinitions
                .Include(b => b.Items)
                .Where(b => b.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of baselines that contain a specific requirement
        /// </summary>
        public async Task<List<BaselineDefinition>> GetListContainingRequirementAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.BaselineDefinitions
                .Include(b => b.Items)
                .Where(b => b.Items.Any(i => i.RequirementId == requirementId))
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of baselines that contain a specific requirement with a specific version
        /// </summary>
        public async Task<List<BaselineDefinition>> GetListContainingRequirementVersionAsync(
            Guid requirementId,
            string version,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.BaselineDefinitions
                .Include(b => b.Items)
                .Where(b => b.Items.Any(i => i.RequirementId == requirementId && i.Version.Value == version))
                .ToListAsync(cancellationToken);
        }
    }
}
