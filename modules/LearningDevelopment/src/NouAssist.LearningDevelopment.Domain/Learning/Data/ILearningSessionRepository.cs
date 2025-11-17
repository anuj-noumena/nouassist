using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.Learning.Data
{
    public interface ILearningSessionRepository : IRepository<LearningSession, Guid>
    {
        Task<LearningSession> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default);
        
        Task<List<LearningSession>> GetActiveSessionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        
        Task<List<LearningSession>> GetPausedSessionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
