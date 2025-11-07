using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CollaborationHub.Resources.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CollaborationHub.Resources.Data
{
    public interface IBookCollectionRepository : IRepository<BookCollection, Guid>
    {
        Task<List<BookCollection>> GetByOwnerAsync(Guid ownerId);
        Task<BookCollection> FindByNameAsync(Guid ownerId, string name);
    }
}
