using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Repositories
{
    public class EfCoreTaskTypeRepository 
        : EfCoreRepository<ITasksDbContext, TaskType, Guid>, ITaskTypeRepository
    {
        public EfCoreTaskTypeRepository(
            IDbContextProvider<ITasksDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaskType> GetWithCustomFieldsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(tt => tt.CustomFields)
                .FirstOrDefaultAsync(tt => tt.Id == id, cancellationToken);
        }

        public async Task<List<TaskType>> GetActiveTaskTypesAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(tt => tt.IsActive)
                .OrderBy(tt => tt.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskType> FindByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .FirstOrDefaultAsync(tt => tt.Name == name && tt.IsActive, 
                    cancellationToken);
        }

        public override async Task<IQueryable<TaskType>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(tt => tt.CustomFields);
        }
    }
}
