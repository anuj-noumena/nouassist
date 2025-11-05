using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Core.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Core.Data
{
    public interface IInboxItemRepository : IRepository<InboxItem, Guid>
    {
        Task<List<InboxItem>> GetUnprocessedByUserAsync(Guid userId);
        Task<List<InboxItem>> GetStaleItemsAsync(Guid userId, int daysToConsiderStale = 7);
        Task<int> CountUnprocessedByUserAsync(Guid userId);
    }

    public interface ITagRepository : IRepository<Tag, Guid>
    {
        Task<List<Tag>> GetByUserAsync(Guid userId);
        Task<List<Tag>> GetPopularTagsAsync(Guid userId, int minUsageCount = 10);
        Task<Tag?> FindByTitleAsync(Guid userId, string title);
    }

    public interface IContextRepository : IRepository<Context, Guid>
    {
        Task<List<Context>> GetActiveByUserAsync(Guid userId);
        Task<List<Context>> GetByLocationAsync(Guid userId, string location);
    }

    public interface IAreaRepository : IRepository<Area, Guid>
    {
        Task<List<Area>> GetByUserAsync(Guid userId);
        Task<List<Area>> GetRootAreasAsync(Guid userId);
        Task<List<Area>> GetChildAreasAsync(Guid parentId);
    }
}
