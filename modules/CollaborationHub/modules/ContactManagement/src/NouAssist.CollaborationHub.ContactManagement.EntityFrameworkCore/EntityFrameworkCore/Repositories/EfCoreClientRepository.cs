using System;
using NouAssist.CollaborationHub.ContactManagement.Aggregates;
using NouAssist.CollaborationHub.ContactManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore.Repositories
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
