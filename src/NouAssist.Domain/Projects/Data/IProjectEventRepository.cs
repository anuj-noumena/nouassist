// src/NouAssist.Projects.Domain/Data/IProjectEventRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.Projects.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.Projects.Data
{
    public interface IProjectEventRepository : IRepository<ProjectEvent, Guid>
    {
        Task<List<ProjectEvent>> GetByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);

        Task<List<ProjectEvent>> GetUpcomingEventsAsync(
            Guid projectId,
            DateTime fromDate,
            CancellationToken cancellationToken = default);
    }
}