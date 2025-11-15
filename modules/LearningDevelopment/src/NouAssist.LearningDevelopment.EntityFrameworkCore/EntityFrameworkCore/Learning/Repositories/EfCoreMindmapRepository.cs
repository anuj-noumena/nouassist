using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Learning.EntityFrameworkCore.Repositories
{
    public class EfCoreMindmapRepository 
        : EfCoreRepository<ILearningDbContext, Mindmap, Guid>, IMindmapRepository
    {
        public EfCoreMindmapRepository(
            IDbContextProvider<ILearningDbContext> dbContextProvider)
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
