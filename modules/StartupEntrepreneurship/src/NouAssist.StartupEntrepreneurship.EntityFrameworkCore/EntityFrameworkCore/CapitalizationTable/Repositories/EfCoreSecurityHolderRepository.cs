using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Aggregates;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Enums;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Repositories;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ISecurityHolderRepository
    /// </summary>
    public class EfCoreSecurityHolderRepository 
        : EfCoreRepository<ICapTableDbContext, SecurityHolder, Guid>, ISecurityHolderRepository
    {
        public EfCoreSecurityHolderRepository(
            IDbContextProvider<ICapTableDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SecurityHolder>> GetListAsync(
            CapTableSpecification<SecurityHolder> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SecurityHolder>> GetByHolderTypeAsync(
            HolderType holderType,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(h => h.HolderType == holderType)
                .ToListAsync(cancellationToken);
        }

        public async Task<SecurityHolder> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(h => h.Name == name, cancellationToken);
        }

        public async Task<SecurityHolder> GetWithVestingSchedulesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(h => h.VestingSchedules)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .AnyAsync(h => h.Name == name, cancellationToken);
        }

        public async Task<SecurityHolder> FindByEmailAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(h => h.Email == email, cancellationToken);
        }

        // Override to include default navigation properties when needed
        public override async Task<IQueryable<SecurityHolder>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(h => h.VestingSchedules);
        }
    }
}
