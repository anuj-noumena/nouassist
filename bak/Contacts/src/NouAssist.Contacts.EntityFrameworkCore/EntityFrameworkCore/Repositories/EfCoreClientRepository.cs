using System;
using NouAssist.Contacts.Aggregates;
using NouAssist.Contacts.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Contacts.EntityFrameworkCore.Repositories
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
