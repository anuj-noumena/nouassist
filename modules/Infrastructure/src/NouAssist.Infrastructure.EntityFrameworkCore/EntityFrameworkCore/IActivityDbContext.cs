using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Activity.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore
{
    [ConnectionStringName(ActivityDbProperties.ConnectionStringName)]
    public interface IActivityDbContext : IEfCoreDbContext
    {
        DbSet<ActivityFeed> ActivityFeeds { get; }
        DbSet<AuditLog> AuditLogs { get; }
        DbSet<EntityHistory> EntityHistories { get; }
    }
}
