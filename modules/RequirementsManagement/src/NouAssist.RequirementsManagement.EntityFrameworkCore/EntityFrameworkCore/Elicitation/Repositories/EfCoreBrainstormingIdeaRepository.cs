using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Repositories
{
    public class EfCoreBrainstormingIdeaRepository 
        : EfCoreRepository<IElicitationDbContext, BrainstormingIdea, Guid>, IBrainstormingIdeaRepository
    {
        public EfCoreBrainstormingIdeaRepository(
            IDbContextProvider<IElicitationDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<List<BrainstormingIdea>> GetBySessionAsync(Guid sessionId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(i => i.SessionId == sessionId).ToListAsync();
        }

        public async Task<List<BrainstormingIdea>> GetSelectedBySessionAsync(Guid sessionId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(i => i.SessionId == sessionId && i.IsSelected).ToListAsync();
        }

        public async Task<List<BrainstormingIdea>> GetByContributorAsync(Guid contributorId)
        {
            var query = await GetQueryableAsync();
            return await query.Where(i => i.ContributorId == contributorId).ToListAsync();
        }

        public async Task<List<BrainstormingIdea>> GetByPriorityRangeAsync(int minPriority, int maxPriority)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.Priority >= minPriority && i.Priority <= maxPriority)
                .OrderByDescending(i => i.Priority)
                .ToListAsync();
        }

        public async Task<List<BrainstormingIdea>> GetByTagAsync(string tagName)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(i => i.Tags.Any(t => t.Name == tagName))
                .ToListAsync();
        }
    }
}
