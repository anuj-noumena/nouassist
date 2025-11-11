using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.ProjectManagement.Data
{
    public interface IProjectSuccessCriterionRepository : IRepository<ProjectSuccessCriterion, Guid>
    {
        Task<List<ProjectSuccessCriterion>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<ProjectSuccessCriterion>> GetUnmetCriteriaAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
    }
}
