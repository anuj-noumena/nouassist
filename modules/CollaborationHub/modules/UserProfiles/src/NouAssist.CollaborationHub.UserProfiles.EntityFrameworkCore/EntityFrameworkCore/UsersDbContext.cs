using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore
{
    [ConnectionStringName(UsersDbProperties.ConnectionStringName)]
    public class UsersDbContext : AbpDbContext<UsersDbContext>, IUsersDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<UserEisenhowerPreference> UserEisenhowerPreferences { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureUsers(this);
        }
    }
}
