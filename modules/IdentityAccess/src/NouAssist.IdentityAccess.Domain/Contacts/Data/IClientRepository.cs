using System;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Contacts.Data
{
    public interface IClientRepository : IRepository<Aggregates.Client, Guid>
    {
    }
}
