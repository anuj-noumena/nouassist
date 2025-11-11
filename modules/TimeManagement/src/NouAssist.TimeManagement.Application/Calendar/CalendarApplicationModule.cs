using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Calendar;

[DependsOn(
    typeof(CalendarDomainModule),
    typeof(CalendarApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class CalendarApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CalendarApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CalendarApplicationModule>(validate: true);
        });
    }
}
