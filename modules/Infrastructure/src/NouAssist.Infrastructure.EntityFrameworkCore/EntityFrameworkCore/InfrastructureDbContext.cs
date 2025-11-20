using Microsoft.EntityFrameworkCore;
using NouAssist.Infrastructure.Activity.Aggregates;
using NouAssist.Infrastructure.Notifications.Aggregates;
using NouAssist.Infrastructure.Reminders.Aggregates;
using NouAssist.Infrastructure.Search.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

[ConnectionStringName(InfrastructureDbProperties.ConnectionStringName)]
public class InfrastructureDbContext : AbpDbContext<InfrastructureDbContext>, IInfrastructureDbContext
{
    // Notifications
    public DbSet<Infrastructure.Notifications.Aggregates.Notification> Notifications { get; set; }
    public DbSet<NotificationPreference> NotificationPreferences { get; set; }
    
    // Reminders
    public DbSet<Reminder> Reminders { get; set; }
    
    // Activity
    public DbSet<ActivityFeed> ActivityFeeds { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<EntityHistory> EntityHistories { get; set; }
    
    // Search
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<SavedSearch> SavedSearches { get; set; }
    public DbSet<PopularSearch> PopularSearches { get; set; }
    public DbSet<SearchAnalytics> SearchAnalytics { get; set; }

    
    public InfrastructureDbContext(DbContextOptions<InfrastructureDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureInfrastructure();
    }
}
