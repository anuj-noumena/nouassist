using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore;

[ConnectionStringName(UserProfilesDbProperties.ConnectionStringName)]
public class UserProfilesDbContext : AbpDbContext<UserProfilesDbContext>, IUserProfilesDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public UserProfilesDbContext(DbContextOptions<UserProfilesDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureUserProfiles();
    }
}
