using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using NouAssist.SkillDevelopment.Localization;

namespace NouAssist.SkillDevelopment
{
    [DependsOn(
        typeof(AbpValidationModule)
    )]
    public class SkillDevelopmentDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<SkillDevelopmentDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<SkillDevelopmentResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization/SkillDevelopment");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("SkillDevelopment", typeof(SkillDevelopmentResource));
            });
        }
    }
}
