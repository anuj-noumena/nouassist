using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of ITaskRepository
    /// </summary>
    public class EfCoreTaskRepository : 
        EfCoreRepository<IProjectExecutionDbContext, Task, Guid>, 
        ITaskRepository
    {
        public EfCoreTaskRepository(
            IDbContextProvider<IProjectExecutionDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
        
        public async Task<Task> GetWithDetailsAsync(
            Guid id,
            bool includeAssignments = true,
            bool includeDependencies = true,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // Include requested navigation properties
            if (includeAssignments)
            {
                query = query.Include(t => t.Assignments);
            }
            
            if (includeDependencies)
            {
                query = query.Include(t => t.Dependencies);
            }
            
            return await query
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }
        
        public async Task<List<Task>> GetListByProjectIdAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(t => t.ProjectId == projectId)
                .OrderBy(t => t.StartDate)
                .ThenBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Task>> GetListByPhaseIdAsync(
            Guid phaseId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(t => t.PhaseId == phaseId)
                .OrderBy(t => t.StartDate)
                .ThenBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Task>> GetListByAssignedUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(t => t.Assignments)
                .Where(t => t.Assignments.Any(a => a.UserId == userId && a.IsActive))
                .OrderBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<List<Task>> GetListByStatusAsync(
            Guid projectId,
            TaskStatus status,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(t => t.ProjectId == projectId && t.Status == status)
                .OrderBy(t => t.DueDate)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<bool> WouldCreateCircularDependencyAsync(
            Guid taskId,
            Guid dependsOnTaskId,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            
            // Recursive CTE to find all dependencies
            var sql = @"
                WITH RECURSIVE task_dependencies AS (
                    SELECT td.""TaskId"", td.""DependsOnTaskId"", 1 AS depth
                    FROM ""proj"".""ProjectExecution_TaskDependencies"" td
                    WHERE td.""TaskId"" = @dependsOnTaskId
                    
                    UNION ALL
                    
                    SELECT td.""TaskId"", td.""DependsOnTaskId"", td_rec.depth + 1
                    FROM ""proj"".""ProjectExecution_TaskDependencies"" td
                    JOIN task_dependencies td_rec ON td.""TaskId"" = td_rec.""DependsOnTaskId""
                    WHERE td_rec.depth < 100  -- Limit recursion depth
                )
                SELECT COUNT(*) FROM task_dependencies 
                WHERE ""DependsOnTaskId"" = @taskId";
                
            // Use FromSqlRaw to execute the CTE
            var param1 = new Npgsql.NpgsqlParameter("@taskId", taskId);
            var param2 = new Npgsql.NpgsqlParameter("@dependsOnTaskId", dependsOnTaskId);
            
            var count = await dbContext.Database
                .ExecuteSqlRawAsync(sql, new[] { param1, param2 }, cancellationToken);
                
            return count > 0;
        }
    }
}
