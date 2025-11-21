using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore.Contacts.Repositories
{
    public class EfCoreContactRepository 
        : EfCoreRepository<IdentityAccessDbContext, Contact, Guid>, IContactRepository
    {
        public EfCoreContactRepository(
            IDbContextProvider<IdentityAccessDbContext> dbContextProvider)
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
