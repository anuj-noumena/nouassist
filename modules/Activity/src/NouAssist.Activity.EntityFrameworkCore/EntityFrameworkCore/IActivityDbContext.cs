using Microsoft.EntityFrameworkCore;
using NouAssist.Activity.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Activity.EntityFrameworkCore
{
    [ConnectionStringName(ActivityDbProperties.ConnectionStringName)]
    public interface IActivityDbContext : IEfCoreDbContext
    {
        DbSet<ActivityFeed> ActivityFeeds { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<EntityHistory> EntityHistories { get; }
    }
}
