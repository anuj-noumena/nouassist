using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.ProjectManagement.Data
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
