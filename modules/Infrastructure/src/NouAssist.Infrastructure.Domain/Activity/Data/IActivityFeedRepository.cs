using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Infrastructure.Activity.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Infrastructure.Activity.Data
{
    public interface IActivityFeedRepository : IRepository<ActivityFeed, Guid>
    {
        Task<List<ActivityFeed>> GetListAsync(ISpecification<ActivityFeed> spec, int maxResultCount = 50);
        Task<long> GetCountAsync(ISpecification<ActivityFeed> spec);
        Task<int> GetUnreadCountAsync(Guid userId);
    }
}
