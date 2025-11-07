using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore
{
    [ConnectionStringName(UsersDbProperties.ConnectionStringName)]
    public interface IUsersDbContext : IEfCoreDbContext
    {
        DbSet<User> Users { get; }
        DbSet<UserSettings> UserSettings { get; }
        DbSet<UserEisenhowerPreference> UserEisenhowerPreferences { get; }
    }
}
