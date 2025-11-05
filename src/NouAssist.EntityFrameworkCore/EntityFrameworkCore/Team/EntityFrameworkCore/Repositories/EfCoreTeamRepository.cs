using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Team.Data;
using NouAssist.Team.Enums;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.Team.EntityFrameworkCore.Repositories
{
    public class EfCoreTeamRepository 
        : EfCoreRepository<ITeamDbContext, Team.Aggregates.Team, Guid>, 
          ITeamRepository
    {
        public EfCoreTeamRepository(
            IDbContextProvider<ITeamDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Team.Aggregates.Team> GetWithMembersAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Teams
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Team.Aggregates.Team>> GetActiveTeamsAsync()
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<Team.Aggregates.Team>> GetTeamsByTypeAsync(TeamType teamType)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(t => t.TeamType == teamType && t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<Team.Aggregates.Team>> GetTeamsByLeaderAsync(Guid leaderId)
        {
            var dbSet = await GetDbSetAsync();
            
            return await dbSet
                .Where(t => t.LeaderId == leaderId && t.IsActive)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<Team.Aggregates.Team>> GetUserTeamsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            
            return await dbContext.Teams
                .Include(t => t.Members)
                .Where(t => t.Members.Any(m => 
                    m.UserId == userId && 
                    m.IsActive))
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<List<Team.Aggregates.Team>> GetListAsync(
            ISpecification<Team.Aggregates.Team> spec)
        {
            var query = await GetQueryableAsync();
            
            return await query
                .Where(spec.ToExpression())
                .ToListAsync();
        }

        public override async Task<Team.Aggregates.Team> GetAsync(
            Guid id, 
            bool includeDetails = true, 
            CancellationToken cancellationToken = default)
        {
            if (includeDetails)
            {
                return await GetWithMembersAsync(id);
            }

            return await base.GetAsync(id, includeDetails, cancellationToken);
        }
    }
}
