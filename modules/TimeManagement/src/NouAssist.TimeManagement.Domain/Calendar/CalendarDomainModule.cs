using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Calendar;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CalendarDomainSharedModule)
)]
public class CalendarDomainModule : AbpModule
{

}
