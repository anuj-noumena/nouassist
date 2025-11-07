using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore;

[ConnectionStringName(UserProfilesDbProperties.ConnectionStringName)]
public interface IUserProfilesDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
