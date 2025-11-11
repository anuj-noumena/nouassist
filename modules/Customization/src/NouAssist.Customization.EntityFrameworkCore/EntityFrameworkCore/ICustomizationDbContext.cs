using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Customization.EntityFrameworkCore;

[ConnectionStringName(CustomizationDbProperties.ConnectionStringName)]
public interface ICustomizationDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
