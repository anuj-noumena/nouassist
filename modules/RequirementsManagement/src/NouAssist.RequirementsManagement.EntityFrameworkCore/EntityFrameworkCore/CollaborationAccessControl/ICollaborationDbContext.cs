using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore
{
    [ConnectionStringName(CollaborationDbProperties.ConnectionStringName)]
    public interface ICollaborationDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root
        DbSet<UserAccessProfile> UserAccessProfiles { get; }
        DbSet<NotificationConfiguration> NotificationConfigurations { get; }
        DbSet<DiscussionBoard> DiscussionBoards { get; }
    }
}
