using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

[ConnectionStringName(InfrastructureDbProperties.ConnectionStringName)]
public interface IInfrastructureDbContext : IEfCoreDbContext
{
    // Notifications
    DbSet<Infrastructure.Notifications.Aggregates.Notification> Notifications { get; }
    DbSet<NotificationPreference> NotificationPreferences { get; }
    
    // Reminders
    DbSet<Reminder> Reminders { get; }
    
    // Activity
    DbSet<ActivityFeed> ActivityFeeds { get; }
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<EntityHistory> EntityHistories { get; }
    
    // Search
    DbSet<SearchHistory> SearchHistories { get; }
    DbSet<SavedSearch> SavedSearches { get; }
    DbSet<PopularSearch> PopularSearches { get; }
    DbSet<SearchAnalytics> SearchAnalytics { get; }
}
