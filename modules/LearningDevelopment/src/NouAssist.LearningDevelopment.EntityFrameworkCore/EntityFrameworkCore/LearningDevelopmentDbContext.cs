using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore;

[ConnectionStringName(LearningDevelopmentDbProperties.ConnectionStringName)]
public class LearningDevelopmentDbContext : AbpDbContext<LearningDevelopmentDbContext>, ILearningDevelopmentDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public LearningDevelopmentDbContext(DbContextOptions<LearningDevelopmentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureLearningDevelopment();
    }
}
