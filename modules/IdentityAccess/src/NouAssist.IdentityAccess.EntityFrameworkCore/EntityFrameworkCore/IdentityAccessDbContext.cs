using Microsoft.EntityFrameworkCore;
using NouAssist.IdentityAccess.Contacts.Aggregates;
using NouAssist.IdentityAccess.Team.Aggregates;
using NouAssist.IdentityAccess.Team.Entities;
using NouAssist.IdentityAccess.Users.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IdentityAccess.EntityFrameworkCore;

[ConnectionStringName(IdentityAccessDbProperties.ConnectionStringName)]
public class IdentityAccessDbContext : AbpDbContext<IdentityAccessDbContext>, IIdentityAccessDbContext
{
    // Contacts
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Client> Clients { get; set; }
    
    // Team
    public DbSet<IdentityAccess.Team.Aggregates.Team> Teams { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    
    // Users
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<UserEisenhowerPreference> UserEisenhowerPreferences { get; set; }

    public IdentityAccessDbContext(DbContextOptions<IdentityAccessDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureIdentityAccess();
    }
}
