using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Calendar;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CalendarDomainSharedModule)
)]
public class CalendarDomainModule : AbpModule
{

}
