using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.Habits.EntityFrameworkCore.Repositories
{
    public class EfCoreRoutineRepository 
        : EfCoreRepository<IHabitsDbContext, Routine, Guid>, IRoutineRepository
    {
        public EfCoreRoutineRepository(
            IDbContextProvider<IHabitsDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Routine> GetWithStepsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<List<Routine>> GetActiveRoutinesAsync(
            Guid ownerId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Include(r => r.Steps)
                .Where(r => r.OwnerId == ownerId && r.IsActive)
                .OrderBy(r => r.TimeOfDay)
                .ThenBy(r => r.Title)
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<Routine>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(r => r.Steps);
        }
    }
}
