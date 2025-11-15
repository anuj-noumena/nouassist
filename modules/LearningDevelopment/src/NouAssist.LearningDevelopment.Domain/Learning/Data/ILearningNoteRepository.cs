using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Learning.Data
{
    public interface ILearningNoteRepository : IRepository<LearningNote, Guid>
    {
        Task<List<LearningNote>> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default);
        
        Task<List<LearningNote>> GetByTypeAsync(
            Guid userId,
            LearningNoteType type,
            CancellationToken cancellationToken = default);
    }
}
