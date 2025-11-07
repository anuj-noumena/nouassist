using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.ReflectionAndSupport.Journaling.EntityFrameworkCore;

[ConnectionStringName(JournalingDbProperties.ConnectionStringName)]
public class JournalingDbContext : AbpDbContext<JournalingDbContext>, IJournalingDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public JournalingDbContext(DbContextOptions<JournalingDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureJournaling();
    }
}
