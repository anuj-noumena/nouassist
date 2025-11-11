// src/NouAssist.CoreProductivity.ProjectManagement.Domain/Data/IProjectRequirementRepository.cs

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.WorkManagement.ProjectManagement.Data
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