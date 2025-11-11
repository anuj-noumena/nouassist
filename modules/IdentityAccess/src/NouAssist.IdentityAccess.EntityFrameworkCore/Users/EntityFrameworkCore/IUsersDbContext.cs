using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.Users.EntityFrameworkCore
{
    [ConnectionStringName(UsersDbProperties.ConnectionStringName)]
    public interface IUsersDbContext : IEfCoreDbContext
    {
        DbSet<User> Users { get; }
        DbSet<UserSettings> UserSettings { get; }
        DbSet<UserEisenhowerPreference> UserEisenhowerPreferences { get; }
    }
}
