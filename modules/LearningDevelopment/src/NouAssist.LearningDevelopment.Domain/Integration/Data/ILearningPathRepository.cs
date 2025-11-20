using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Integration.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.Integration.Data
{
    public interface ILearningPathRepository : IRepository<LearningPath, Guid>
    {
        Task<List<LearningPath>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<LearningPath>> GetByStatusAsync(
            Guid userId,
            LearningPathStatus status,
            CancellationToken cancellationToken = default);

        Task<LearningPath> FindByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default);

        Task<LearningPath> FindBySkillIdAsync(
            Guid skillId,
            CancellationToken cancellationToken = default);

        Task<List<LearningPath>> GetActivePathsAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<LearningPath>> GetInMaintenancePhaseAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
