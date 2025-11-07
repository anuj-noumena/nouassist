using System;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CollaborationHub.ContactManagement.Data
{
    public interface IClientRepository : IRepository<Aggregates.Client, Guid>
    {
    }
}
