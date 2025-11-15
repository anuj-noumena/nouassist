using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories
{
    public class EfCoreSkillDevelopmentSessionRepository 
        : EfCoreRepository<ISkillDevelopmentDbContext, SkillDevelopmentSession, Guid>, 
          ISkillDevelopmentSessionRepository
    {
        public EfCoreSkillDevelopmentSessionRepository(
            IDbContextProvider<ISkillDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<SkillDevelopmentSession> GetBySkillIdAsync(
            Guid skillId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .FirstOrDefaultAsync(s => s.SkillId == skillId, cancellationToken);
        }

        public async Task<List<SkillDevelopmentSession>> GetActiveSessionsByUserIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(s => s.UserId == userId && !s.Completed && !s.Paused)
                .OrderByDescending(s => s.LastActivity)
                .ToListAsync(cancellationToken);
        }

        // Override to ensure owned entities are loaded
        public override async Task<SkillDevelopmentSession> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            // EF Core automatically loads owned entities (Milestones)
            // No need for explicit Include for OwnsMany
            return await query
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}
