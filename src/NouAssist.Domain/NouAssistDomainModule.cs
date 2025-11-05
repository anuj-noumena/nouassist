using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NouAssist.Activity.BackgroundWorkers;
using NouAssist.Localization;
using NouAssist.MultiTenancy;
using NouAssist.Notifications.BackgroundWorkers;
using NouAssist.Reminders.BackgroundWorkers;
using NouAssist.Resources.BackgroundWorkers;
using NouAssist.Search.BackgroundWorkers;
using Volo.Abp;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Caching;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace NouAssist;

[DependsOn(
    typeof(NouAssistDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpCachingModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(BlobStoringDatabaseDomainModule),
    typeof(AbpBackgroundJobsModule),
    typeof(AbpBackgroundWorkersModule)
    )]
public class NouAssistDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });
        var configuration = context.Services.GetConfiguration();
        context.Services.AddAbpHangfire(hangfire =>
        {
            hangfire.UsePostgreSqlStorage(configuration.GetConnectionString("Hangfire"));
        });


#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }
    
    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<AuditLogArchiveWorker>();
        await context.AddBackgroundWorkerAsync<ExpiredNotificationCleanupWorker>();
        await context.AddBackgroundWorkerAsync<NotificationRetryWorker>();
        await context.AddBackgroundWorkerAsync<ReminderProcessingWorker>();
        await context.AddBackgroundWorkerAsync<InventoryMaintenanceReminderWorker>();
        await context.AddBackgroundWorkerAsync<SearchIndexOptimizationWorker>();
        await context.AddBackgroundWorkerAsync<SearchAnalyticsAggregationWorker>();
    }

}
