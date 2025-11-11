using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Calendar;

[DependsOn(
    typeof(CalendarDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CalendarApplicationContractsModule : AbpModule
{

}
