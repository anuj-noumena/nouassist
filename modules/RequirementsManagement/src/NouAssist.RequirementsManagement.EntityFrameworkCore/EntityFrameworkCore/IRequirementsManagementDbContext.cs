using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.EntityFrameworkCore;

[ConnectionStringName(RequirementsManagementDbProperties.ConnectionStringName)]
public interface IRequirementsManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
