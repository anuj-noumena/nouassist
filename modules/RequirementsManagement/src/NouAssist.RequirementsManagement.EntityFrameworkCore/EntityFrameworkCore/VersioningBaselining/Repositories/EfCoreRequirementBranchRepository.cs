using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.Exceptions;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.VersioningBaselining.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of IRequirementBranchRepository
    /// </summary>
    public class EfCoreRequirementBranchRepository :
        EfCoreRepository<IVersionControlDbContext, RequirementBranch, Guid>,
        IRequirementBranchRepository
    {
        public EfCoreRequirementBranchRepository(
            IDbContextProvider<IVersionControlDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets a branch by ID with its merge operations
        /// </summary>
        public override async Task<RequirementBranch> GetAsync(
            Guid id,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.RequirementBranches.AsQueryable();
            
            if (includeDetails)
            {
                query = query.Include(b => b.MergeOperations);
            }
            
            var branch = await query
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (branch == null)
            {
                throw new BranchNotFoundException(id);
            }
            
            return branch;
        }

        /// <summary>
        /// Checks if a branch with the given name already exists
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            string name,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = dbContext.RequirementBranches.AsQueryable();
            
            if (excludeId.HasValue)
            {
                query = query.Where(b => b.Id != excludeId.Value);
            }
            
            return await query
                .AnyAsync(b => EF.Property<string>(b, "Name") == name, cancellationToken);
        }

        /// <summary>
        /// Gets a branch by its name
        /// </summary>
        public async Task<RequirementBranch> GetByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var branch = await dbContext.RequirementBranches
                .Include(b => b.MergeOperations)
                .Where(b => EF.Property<string>(b, "Name") == name)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (branch == null)
            {
                throw new BranchNameAlreadyExistsException(name);
            }
            
            return branch;
        }

        /// <summary>
        /// Gets a list of branches by status
        /// </summary>
        public async Task<List<RequirementBranch>> GetListByStatusAsync(
            BranchStatus status,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.RequirementBranches
                .Include(b => b.MergeOperations)
                .Where(b => b.Status == status)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets a list of branches created from a specific source
        /// </summary>
        public async Task<List<RequirementBranch>> GetListBySourceAsync(
            Guid sourceId,
            string sourceType,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.RequirementBranches
                .Include(b => b.MergeOperations)
                .Where(b => b.SourceId == sourceId && b.SourceType == sourceType)
                .ToListAsync(cancellationToken);
        }
    }
}
