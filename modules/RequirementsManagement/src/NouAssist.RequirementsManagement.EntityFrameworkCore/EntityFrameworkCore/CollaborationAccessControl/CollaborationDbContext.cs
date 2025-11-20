using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore
{
    [ConnectionStringName(CollaborationDbProperties.ConnectionStringName)]
    public class CollaborationDbContext : AbpDbContext<CollaborationDbContext>, ICollaborationDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<UserAccessProfile> UserAccessProfiles { get; set; }
        public DbSet<NotificationConfiguration> NotificationConfigurations { get; set; }
        public DbSet<DiscussionBoard> DiscussionBoards { get; set; }

        public CollaborationDbContext(DbContextOptions<CollaborationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly using the extension method
            builder.ConfigureCollaboration();
        }
    }
}
