using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Data;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore.Repositories
{
    public class EfCoreSkillLogRepository 
        : EfCoreRepository<ISkillDevelopmentDbContext, SkillLog, Guid>, ISkillLogRepository
    {
        public EfCoreSkillLogRepository(
            IDbContextProvider<ISkillDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<SkillLog>> GetBySkillIdAsync(
            Guid skillId, 
            SkillLogType? type = null, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            query = query.Where(l => l.SkillId == skillId);
            
            if (type.HasValue)
            {
                query = query.Where(l => l.Type == type.Value);
            }
            
            return await query
                .OrderByDescending(l => l.CreationTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SkillLog>> GetRecentByUserIdAsync(
            Guid userId, 
            int count = 10, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreationTime)
                .Take(count)
                .ToListAsync(cancellationToken);
        }
    }
}
