using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.Repositories
{
    /// <summary>
    /// Repository for TimeLog aggregate
    /// </summary>
    public interface ITimeLogRepository : IRepository<TimeLog, Guid>
    {
        /// <summary>
        /// Gets a list of time logs by project ID
        /// </summary>
        Task<List<TimeLog>> GetListByProjectIdAsync(
            Guid projectId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of time logs by task ID
        /// </summary>
        Task<List<TimeLog>> GetListByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a list of time logs by user ID
        /// </summary>
        Task<List<TimeLog>> GetListByUserIdAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets total logged hours for a project
        /// </summary>
        Task<double> GetTotalHoursByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets total logged hours for a task
        /// </summary>
        Task<double> GetTotalHoursByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default);
    }
}
