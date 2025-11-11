using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.FinancialManagement.EntityFrameworkCore;

[ConnectionStringName(FinancialManagementDbProperties.ConnectionStringName)]
public interface IFinancialManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
