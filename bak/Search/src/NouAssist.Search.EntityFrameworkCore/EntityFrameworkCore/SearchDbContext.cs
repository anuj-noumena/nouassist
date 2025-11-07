using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Search.EntityFrameworkCore;

[ConnectionStringName(SearchDbProperties.ConnectionStringName)]
public class SearchDbContext : AbpDbContext<SearchDbContext>, ISearchDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public SearchDbContext(DbContextOptions<SearchDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureSearch();
    }
}
