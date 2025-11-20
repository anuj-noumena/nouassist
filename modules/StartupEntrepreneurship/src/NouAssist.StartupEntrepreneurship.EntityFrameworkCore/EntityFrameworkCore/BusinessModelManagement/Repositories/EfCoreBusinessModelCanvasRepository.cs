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
    public class EfCoreBusinessModelCanvasRepository 
        : EfCoreRepository<IBusinessModelManagementDbContext, BusinessModelCanvas, Guid>, 
          IBusinessModelCanvasRepository
    {
        public EfCoreBusinessModelCanvasRepository(
            IDbContextProvider<IBusinessModelManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<BusinessModelCanvas> FindByTitleAsync(
            string title, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(c => c.Title == title && c.TenantId == tenantId, cancellationToken);
        }

        public async Task<List<BusinessModelCanvas>> GetByOwnerAsync(
            Guid ownerId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(c => c.OwnerId == ownerId)
                .OrderByDescending(c => c.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<BusinessModelCanvas>> GetWithValidationStatusAsync(
            ValidationStatus status, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(c => c.TenantId == tenantId && 
                           c.Components.Any(comp => comp.ValidationState.Status == status))
                .ToListAsync(cancellationToken);
        }

        public async Task<BusinessModelCanvas> GetWithVersionsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(c => c.Versions)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<BusinessModelCanvas> GetWithComponentsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(c => c.Components)
                    .ThenInclude(comp => comp.Hypotheses)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<BusinessModelCanvas> GetWithVersionsAndComponentsAsync(
            Guid id, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Include(c => c.Versions)
                .Include(c => c.Components)
                    .ThenInclude(comp => comp.Hypotheses)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<List<BusinessModelCanvas>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<BusinessModelCanvas> specification,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public override async Task<BusinessModelCanvas> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetWithVersionsAndComponentsAsync(id, cancellationToken);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }

        public override async Task<List<BusinessModelCanvas>> GetListAsync(
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                var query = await GetQueryableAsync();
                return await query
                    .Include(c => c.Versions)
                    .Include(c => c.Components)
                        .ThenInclude(comp => comp.Hypotheses)
                    .ToListAsync(cancellationToken);
            }

            return await base.GetListAsync(includeDetails, cancellationToken);
        }

        public async Task<List<BusinessModelCanvas>> GetPagedListAsync(
            int skipCount,
            int maxResultCount,
            string sorting = null,
            Guid? ownerId = null,
            ValidationStatus? validationStatus = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            if (ownerId.HasValue)
            {
                query = query.Where(c => c.OwnerId == ownerId.Value);
            }

            if (validationStatus.HasValue)
            {
                query = query.Where(c => c.Components.Any(comp => comp.ValidationState.Status == validationStatus.Value));
            }

            query = query
                .OrderBy(sorting.IsNullOrWhiteSpace() ? nameof(BusinessModelCanvas.CreationTime) + " desc" : sorting)
                .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
