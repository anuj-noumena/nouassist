using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(PersonalDevelopmentDomainModule),
    typeof(PersonalDevelopmentTestBaseModule)
)]
public class PersonalDevelopmentDomainTestModule : AbpModule
{

}
