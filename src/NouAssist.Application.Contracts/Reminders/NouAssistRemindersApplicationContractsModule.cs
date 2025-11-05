using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Reminders
{
    [DependsOn(
        typeof(NouAssistRemindersDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
    )]
    public class NouAssistRemindersApplicationContractsModule : AbpModule
    {
    }
}

// ===========================================
// Resources Modules
// ===========================================
