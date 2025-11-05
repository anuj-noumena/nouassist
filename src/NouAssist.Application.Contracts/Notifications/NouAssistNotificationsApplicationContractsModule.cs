using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Notifications
{
    [DependsOn(
        typeof(NouAssistNotificationsDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class NouAssistNotificationsApplicationContractsModule : AbpModule
    {
    }
}

// ===========================================
// Reminders Modules
// ===========================================
