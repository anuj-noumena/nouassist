using System;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.Contacts.EntityFrameworkCore.Repositories
{
    public class EfCoreClientRepository 
        : EfCoreRepository<IContactsDbContext, Client, Guid>, IClientRepository
    {
        public EfCoreClientRepository(
            IDbContextProvider<IContactsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
