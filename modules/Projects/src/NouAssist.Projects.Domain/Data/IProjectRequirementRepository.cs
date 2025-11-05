// src/NouAssist.Projects.Domain/Data/IProjectRequirementRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Projects.Entities;
using NouAssist.Workflow.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Projects.Data
{
    public interface IProjectRequirementRepository : IRepository<ProjectRequirement, Guid>
    {
        Task<List<ProjectRequirement>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<ProjectRequirement>> GetPendingApprovalAsync(
            CancellationToken cancellationToken = default);
    }
}