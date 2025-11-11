using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Infrastructure.Search.Data
{
    public interface ISavedSearchRepository : IRepository<SavedSearch, Guid>
    {
        Task<List<SavedSearch>> GetByUserAsync(Guid userId);
        Task<SavedSearch> FindByNameAsync(Guid userId, string name);
    }
}
