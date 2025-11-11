using System;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IdentityAccess.Contacts.Data
{
    public interface IClientRepository : IRepository<Aggregates.Client, Guid>
    {
    }
}
