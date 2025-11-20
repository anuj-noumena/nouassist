using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class OrganizationRepository : EfCoreRepository<CRMDbContext, Organization, Guid>, IOrganizationRepository
    {
        public OrganizationRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> ExistsByNameAsync(
            string name, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.Organizations
                .Where(o => o.Name == name);
                
            if (excludeId.HasValue)
            {
                query = query.Where(o => o.Id != excludeId.Value);
            }
            
            return await query.AnyAsync(cancellationToken);
        }

        public async Task<List<Organization>> GetByTypeAsync(
            OrganizationType type, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Organizations
                .Where(o => EF.Property<OrganizationType>(o.Details, "Type") == type)
                .OrderBy(sorting ?? nameof(Organization.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Organization>> GetChildrenAsync(
            Guid parentId, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Organizations
                .Where(o => o.ParentOrganizationId == parentId)
                .OrderBy(sorting ?? nameof(Organization.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Organization>> GetByTagAsync(
            string tag, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Organizations
                .Where(o => o.Tags.Contains(tag))
                .OrderBy(sorting ?? nameof(Organization.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
