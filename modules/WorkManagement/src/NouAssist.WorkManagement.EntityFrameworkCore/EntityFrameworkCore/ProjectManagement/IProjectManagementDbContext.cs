using NouAssist.WorkManagement.ProjectManagement;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement;

[ConnectionStringName(ProjectManagementDbProperties.ConnectionStringName)]
public interface IProjectManagementDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
