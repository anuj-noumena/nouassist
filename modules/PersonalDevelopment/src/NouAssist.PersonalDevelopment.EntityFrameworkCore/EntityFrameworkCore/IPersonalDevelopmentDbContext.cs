using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.PersonalDevelopment.EntityFrameworkCore;

[ConnectionStringName(PersonalDevelopmentDbProperties.ConnectionStringName)]
public interface IPersonalDevelopmentDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
