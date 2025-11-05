using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using NouAssist.Activity.Localization;

namespace NouAssist.Activity
{
    public class NouAssistActivityDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<ActivityResource>("en")
                    .AddVirtualJson("/Localization/Activity");
            });
        }
    }
}
