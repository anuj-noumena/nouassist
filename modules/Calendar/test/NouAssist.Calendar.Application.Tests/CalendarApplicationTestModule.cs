using Volo.Abp.Modularity;

namespace NouAssist.Calendar;

[DependsOn(
    typeof(CalendarApplicationModule),
    typeof(CalendarDomainTestModule)
    )]
public class CalendarApplicationTestModule : AbpModule
{

}
