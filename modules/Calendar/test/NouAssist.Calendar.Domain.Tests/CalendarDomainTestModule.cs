using Volo.Abp.Modularity;

namespace NouAssist.Calendar;

[DependsOn(
    typeof(CalendarDomainModule),
    typeof(CalendarTestBaseModule)
)]
public class CalendarDomainTestModule : AbpModule
{

}
