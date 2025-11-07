using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using NouAssist.CollaborationHub.ContactManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreContactRepository 
        : EfCoreRepository<IContactsDbContext, Contact, Guid>, IContactRepository
    {
        public EfCoreContactRepository(
            IDbContextProvider<IContactsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<Contact>> GetContactsNeedingFollowupAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(c => c.OwnerId == ownerId && 
                           c.IsActive &&
                           c.NextFollowupDate.HasValue &&
                           c.NextFollowupDate.Value <= now)
                .OrderBy(c => c.NextFollowupDate)
                .ToListAsync(cancellationToken);
        }
    }
}
