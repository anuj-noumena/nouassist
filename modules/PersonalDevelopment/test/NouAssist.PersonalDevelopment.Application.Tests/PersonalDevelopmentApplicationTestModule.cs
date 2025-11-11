using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(PersonalDevelopmentApplicationModule),
    typeof(PersonalDevelopmentDomainTestModule)
    )]
public class PersonalDevelopmentApplicationTestModule : AbpModule
{

}
