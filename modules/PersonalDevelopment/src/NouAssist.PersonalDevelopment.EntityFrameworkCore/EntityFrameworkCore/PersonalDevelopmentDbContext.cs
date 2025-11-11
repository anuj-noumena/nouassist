using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

[ConnectionStringName(PersonalDevelopmentDbProperties.ConnectionStringName)]
public class PersonalDevelopmentDbContext : AbpDbContext<PersonalDevelopmentDbContext>, IPersonalDevelopmentDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public PersonalDevelopmentDbContext(DbContextOptions<PersonalDevelopmentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigurePersonalDevelopment();
    }
}
