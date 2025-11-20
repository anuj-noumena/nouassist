using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories
{
    public class EfCoreMentorProfileRepository : 
        EfCoreRepository<KnowledgeInnovationDbContext, MentorProfile, Guid>, 
        IMentorProfileRepository
    {
        public EfCoreMentorProfileRepository(
            IDbContextProvider<KnowledgeInnovationDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<List<MentorProfile>> GetListAsync(
            Specification<MentorProfile> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(
            Specification<MentorProfile> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .CountAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(
            Specification<MentorProfile> specification, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(specification.ToExpression())
                .AnyAsync(cancellationToken);
        }

        public async Task<List<MentorProfile>> GetByExpertiseAsync(
            string expertiseKeyword, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(m => EF.Functions.ILike(m.Expertise, $"%{expertiseKeyword}%"))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MentorProfile>> GetExperiencedMentorsAsync(
            int minimumYears, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(m => m.ExperienceYears >= minimumYears)
                .OrderByDescending(m => m.ExperienceYears)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<MentorProfile>> GetByTagAsync(
            string tag, 
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            return await query
                .Where(m => m.Tags.Contains(tag))
                .ToListAsync(cancellationToken);
        }

        public override async Task<IQueryable<MentorProfile>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(m => m.AdviceRecords);
        }
    }
}
