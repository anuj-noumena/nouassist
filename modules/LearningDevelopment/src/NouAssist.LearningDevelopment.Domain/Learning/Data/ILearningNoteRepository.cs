using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.Learning.Data
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
