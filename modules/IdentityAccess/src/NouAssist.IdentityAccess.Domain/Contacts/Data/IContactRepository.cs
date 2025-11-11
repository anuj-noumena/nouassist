using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IdentityAccess.Contacts.Data
{
    public interface IContactRepository : IRepository<Aggregates.Contact, Guid>
    {
        Task<List<Aggregates.Contact>> GetContactsNeedingFollowupAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default);
    }
}
