using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs
{
    /// <summary>
    /// Interface for time log application service
    /// </summary>
    public interface ITimeLogAppService : 
        ICrudAppService<
            TimeLogDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateTimeLogDto,
            UpdateTimeLogDto>
    {
        /// <summary>
        /// Gets a list of time logs by project ID
        /// </summary>
        Task<List<TimeLogDto>> GetListByProjectIdAsync(Guid projectId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets a list of time logs by task ID
        /// </summary>
        Task<List<TimeLogDto>> GetListByTaskIdAsync(Guid taskId);
        
        /// <summary>
        /// Gets a list of time logs by user ID
        /// </summary>
        Task<List<TimeLogDto>> GetListByUserIdAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets time log summary for a project
        /// </summary>
        Task<TimeLogSummaryDto> GetProjectSummaryAsync(Guid projectId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets time log summary for a user
        /// </summary>
        Task<TimeLogSummaryDto> GetUserSummaryAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null);
    }
}
using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs
{
    /// <summary>
    /// Interface for time log application service
    /// </summary>
    public interface ITimeLogAppService : IApplicationService
    {
        /// <summary>
        /// Gets a time log by ID
        /// </summary>
        Task<TimeLogDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paged list of time logs
        /// </summary>
        Task<PagedResultDto<TimeLogDto>> GetListAsync(GetTimeLogListDto input);
        
        /// <summary>
        /// Gets a paged list of time logs for the current user
        /// </summary>
        Task<PagedResultDto<TimeLogDto>> GetMyTimeLogsAsync(GetTimeLogListDto input);
        
        /// <summary>
        /// Creates a new time log
        /// </summary>
        Task<TimeLogDto> CreateAsync(CreateTimeLogDto input);
        
        /// <summary>
        /// Updates an existing time log
        /// </summary>
        Task<TimeLogDto> UpdateAsync(Guid id, UpdateTimeLogDto input);
        
        /// <summary>
        /// Deletes a time log
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Gets a summary of time logs for a project
        /// </summary>
        Task<TimeLogSummaryDto> GetProjectSummaryAsync(Guid projectId, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Gets a summary of time logs for a task
        /// </summary>
        Task<TimeLogSummaryDto> GetTaskSummaryAsync(Guid taskId);
        
        /// <summary>
        /// Gets a summary of time logs for the current user
        /// </summary>
        Task<TimeLogSummaryDto> GetMyTimeLogSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
    }
}
