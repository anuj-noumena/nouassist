// src/NouAssist.CoreProductivity.ProjectManagement.Domain/Data/IProjectRequirementRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.Workflow.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CoreProductivity.ProjectManagement.Data
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