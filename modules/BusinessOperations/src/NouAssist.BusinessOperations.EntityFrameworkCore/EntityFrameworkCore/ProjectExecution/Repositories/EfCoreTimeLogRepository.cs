using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ITimeLogRepository
    /// </summary>
    public class EfCoreTimeLogRepository : 
        EfCoreRepository<IProjectExecutionDbContext, TimeLog, Guid>, 
        ITimeLogRepository
    {
        public EfCoreTimeLogRepository(
            IDbContextProvider<IProjectExecutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        
        public async Task<List<TimeLog>> GetListByProjectIdAsync(
            Guid projectId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(tl => tl.ProjectId == projectId);
            
            if (startDate.HasValue)
            {
                query = query.Where(tl => tl.Details.LogDate >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                query = query.Where(tl => tl.Details.LogDate <= endDate.Value);
            }
            
            return await query
                .OrderBy(tl => tl.Details.LogDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TimeLog>> GetListByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(tl => tl.TaskId == taskId)
                .OrderBy(tl => tl.Details.LogDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<TimeLog>> GetListByUserIdAsync(
            Guid userId,
            DateTime? startDate = null,
            DateTime? endDate = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(tl => tl.UserId == userId);
            
            if (startDate.HasValue)
            {
                query = query.Where(tl => tl.Details.LogDate >= startDate.Value);
            }
            
            if (endDate.HasValue)
            {
                query = query.Where(tl => tl.Details.LogDate <= endDate.Value);
            }
            
            return await query
                .OrderBy(tl => tl.Details.LogDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<double> GetTotalHoursByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(tl => tl.ProjectId == projectId)
                .SumAsync(tl => tl.Details.Hours, cancellationToken);
        }
        
        public async Task<double> GetTotalHoursByTaskIdAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(tl => tl.TaskId == taskId)
                .SumAsync(tl => tl.Details.Hours, cancellationToken);
        }
    }
}
