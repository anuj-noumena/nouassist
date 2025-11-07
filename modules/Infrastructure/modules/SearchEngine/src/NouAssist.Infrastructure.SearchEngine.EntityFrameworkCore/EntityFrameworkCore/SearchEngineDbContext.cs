using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.SearchEngine.EntityFrameworkCore;

[ConnectionStringName(SearchEngineDbProperties.ConnectionStringName)]
public class SearchEngineDbContext : AbpDbContext<SearchEngineDbContext>, ISearchEngineDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public SearchEngineDbContext(DbContextOptions<SearchEngineDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSearchEngine();
    }
}
