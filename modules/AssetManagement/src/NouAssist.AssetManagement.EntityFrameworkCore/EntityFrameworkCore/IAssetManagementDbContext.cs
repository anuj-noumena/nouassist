using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

[ConnectionStringName(AssetManagementDbProperties.ConnectionStringName)]
public interface IAssetManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
