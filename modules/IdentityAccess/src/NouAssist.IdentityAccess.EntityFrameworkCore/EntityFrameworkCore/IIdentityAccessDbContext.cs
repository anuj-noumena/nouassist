using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Team.Aggregates;
using NouAssist.IdentityAccess.Team.Entities;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

[ConnectionStringName(IdentityAccessDbProperties.ConnectionStringName)]
public interface IIdentityAccessDbContext : IEfCoreDbContext
{
    // Contacts
    DbSet<Contact> Contacts { get; }
    DbSet<Client> Clients { get; }
    
    // Team
    DbSet<IdentityAccess.Team.Aggregates.Team> Teams { get; }
    DbSet<Role> Roles { get; }
    DbSet<TeamMember> TeamMembers { get; }
    DbSet<UserRole> UserRoles { get; }
    
    // User
    DbSet<User> Users { get; }
    DbSet<UserSettings> UserSettings { get; }
    DbSet<UserEisenhowerPreference> UserEisenhowerPreferences { get; }
}
