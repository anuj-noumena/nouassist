using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    public class ContactRepository : EfCoreRepository<CRMDbContext, Contact, Guid>, IContactRepository
    {
        public ContactRepository(IDbContextProvider<CRMDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<bool> ExistsByEmailAsync(
            string email, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.Contacts
                .Where(c => EF.Property<string>(c.ContactInfo, "Email") == email);
                
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }
            
            return await query.AnyAsync(cancellationToken);
        }

        public async Task<Contact> FindByEmailAsync(
            string email, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contacts
                .Where(c => EF.Property<string>(c.ContactInfo, "Email") == email)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Contact>> GetByOrganizationAsync(
            Guid organizationId, 
            bool primaryOnly = false, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            var query = dbContext.Contacts
                .Where(c => c.OrganizationId == organizationId);
                
            if (primaryOnly)
            {
                query = query.Where(c => c.IsPrimaryContact);
            }
            
            return await query
                .OrderBy(sorting ?? nameof(Contact.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Contact>> GetByGroupAsync(
            Guid groupId, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // Since GroupIds is stored as JSON, we need to use a custom approach
            // This is a simplified version and may need adjustment based on the actual DB provider
            string groupIdString = groupId.ToString();
            
            return await dbContext.Contacts
                .Where(c => c.GroupIds.Any(g => g == groupId))
                .OrderBy(sorting ?? nameof(Contact.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Contact>> GetByTagAsync(
            string tag, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Contacts
                .Where(c => c.Tags.Contains(tag))
                .OrderBy(sorting ?? nameof(Contact.Name))
                .Skip(skipCount)
                .Take(maxResultCount)
                .ToListAsync(cancellationToken);
        }
    }
}
