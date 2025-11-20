using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using NouAssist.Infrastructure.Search.Aggregates;

namespace NouAssist.Infrastructure.Search.Data
{
    public interface ISavedSearchRepository : IRepository<SavedSearch, Guid>
    {
        Task<List<SavedSearch>> GetByUserAsync(Guid userId);
        Task<SavedSearch> FindByNameAsync(Guid userId, string name);
    }
}