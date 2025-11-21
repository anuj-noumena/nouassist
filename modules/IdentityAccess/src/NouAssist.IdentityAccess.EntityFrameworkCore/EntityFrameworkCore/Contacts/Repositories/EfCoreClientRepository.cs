using System;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Contacts.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore.Contacts.Repositories
{
    public class EfCoreClientRepository 
        : EfCoreRepository<IdentityAccessDbContext, Client, Guid>, IClientRepository
    {
        public EfCoreClientRepository(
            IDbContextProvider<IdentityAccessDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
