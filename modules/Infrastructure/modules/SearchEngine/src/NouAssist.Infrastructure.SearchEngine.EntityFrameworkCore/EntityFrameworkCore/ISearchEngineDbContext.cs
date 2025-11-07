using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.SearchEngine.EntityFrameworkCore;

[ConnectionStringName(SearchEngineDbProperties.ConnectionStringName)]
public interface ISearchEngineDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
