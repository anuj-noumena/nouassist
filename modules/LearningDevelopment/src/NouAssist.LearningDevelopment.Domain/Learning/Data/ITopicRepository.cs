using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Learning.Aggregates;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace NouAssist.Learning.Data
{
    public interface ITopicRepository : IRepository<Topic, Guid>
    {
        Task<Topic> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<Topic>> GetUserTopicsAsync(
            Guid userId,
            ISpecification<Topic> spec = null,
            CancellationToken cancellationToken = default);
        
        Task<List<Topic>> GetRootTopicsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        Task<List<Topic>> GetChildrenAsync(
            Guid parentId,
            CancellationToken cancellationToken = default);
    }
}
