using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.Learning.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning.Repositories
{
    public class EfCoreMindmapRepository 
        : EfCoreRepository<ILearningDevelopmentDbContext, Mindmap, Guid>, IMindmapRepository
    {
        public EfCoreMindmapRepository(
            IDbContextProvider<ILearningDevelopmentDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Mindmap> GetByTopicIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .FirstOrDefaultAsync(m => m.TopicId == topicId, cancellationToken);
        }
    }
}
