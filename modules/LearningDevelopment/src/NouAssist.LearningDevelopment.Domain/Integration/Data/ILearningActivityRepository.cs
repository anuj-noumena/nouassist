using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.LearningDevelopment.Integration.Data
{
    public interface ILearningActivityRepository : IRepository<LearningActivity, Guid>
    {
        Task<LearningActivity> FindByLearningPathIdAsync(
            Guid learningPathId,
            CancellationToken cancellationToken = default);

        Task<List<LearningActivity>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<List<LearningActivity>> GetActiveActivitiesAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
    }
}
