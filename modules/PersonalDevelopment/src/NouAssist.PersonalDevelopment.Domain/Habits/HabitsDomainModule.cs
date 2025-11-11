using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.Habits;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(HabitsDomainSharedModule)
)]
public class HabitsDomainModule : AbpModule
{

}
