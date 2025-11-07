using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.ContactManagement.EntityFrameworkCore;

[ConnectionStringName(ContactManagementDbProperties.ConnectionStringName)]
public interface IContactManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
