using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreRevenueModelRepository 
        : EfCoreRepository<IBusinessModelManagementDbContext, RevenueModel, Guid>, 
          IRevenueModelRepository
    {
        public EfCoreRevenueModelRepository(
            IDbContextProvider<IBusinessModelManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<RevenueModel> FindByNameAsync(
            string name, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(rm => rm.Name == name && rm.TenantId == tenantId, cancellationToken);
        }

        public async Task<List<RevenueModel>> GetByOwnerAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rm => rm.OwnerId == ownerId)
                .OrderByDescending(rm => rm.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RevenueModel>> GetByTypeAsync(
            RevenueModelType type, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rm => rm.Type == type && rm.TenantId == tenantId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<RevenueModel>> GetActiveModelsAsync(
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(rm => rm.IsActive && rm.TenantId == tenantId)
                .ToListAsync(cancellationToken);
        }

        public async Task<RevenueModel> GetWithPricingModelsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(rm => rm.PricingModels)
                .FirstOrDefaultAsync(rm => rm.Id == id, cancellationToken);
        }

        public async Task<RevenueModel> GetWithScenariosAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(rm => rm.Scenarios)
                .FirstOrDefaultAsync(rm => rm.Id == id, cancellationToken);
        }

        public async Task<RevenueModel> GetWithGtmStrategiesAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(rm => rm.GtmStrategies)
                .FirstOrDefaultAsync(rm => rm.Id == id, cancellationToken);
        }

        public async Task<RevenueModel> GetCompleteAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(rm => rm.PricingModels)
                .Include(rm => rm.Scenarios)
                .Include(rm => rm.GtmStrategies)
                .FirstOrDefaultAsync(rm => rm.Id == id, cancellationToken);
        }

        public async Task<List<RevenueModel>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<RevenueModel> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<RevenueModel> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetCompleteAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }

        public async Task<List<RevenueModel>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting = null,
            Guid? ownerId = null,
            RevenueModelType? type = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (ownerId.HasValue)
            {
                query = query.Where(rm => rm.OwnerId == ownerId.Value);
            }

            if (type.HasValue)
            {
                query = query.Where(rm => rm.Type == type.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(rm => rm.IsActive == isActive.Value);
            }

            query = query
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(RevenueModel.CreationTime) + " desc" : sorting)
                .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
