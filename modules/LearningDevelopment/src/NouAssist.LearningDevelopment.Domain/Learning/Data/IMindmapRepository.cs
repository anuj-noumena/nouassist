using System;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.Learning.Data
{
    public interface IMindmapRepository : IRepository<Mindmap, Guid>
    {
        Task<Mindmap> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default);
    }
}
