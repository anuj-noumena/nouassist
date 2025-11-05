using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace NouAssist.Calendar
{
    [DependsOn(
        typeof(CalendarDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule)
    )]
    public class CalendarApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register FluentValidation validators
            context.Services.AddValidatorsFromAssembly(typeof(CalendarApplicationContractsModule).Assembly);
        }
    }
}
