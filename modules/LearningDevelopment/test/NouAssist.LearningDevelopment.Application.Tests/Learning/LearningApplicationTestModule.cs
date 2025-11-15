using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NouAssist.Learning
{
    [DependsOn(
        typeof(LearningApplicationModule),
        typeof(LearningDomainTestModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpAutofacModule)
    )]
    public class LearningApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Mock authorization
            context.Services.AddAlwaysAllowAuthorization();
        }
    }
}
