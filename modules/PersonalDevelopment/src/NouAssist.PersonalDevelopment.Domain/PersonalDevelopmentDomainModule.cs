using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PersonalDevelopmentDomainSharedModule)
)]
public class PersonalDevelopmentDomainModule : AbpModule
{

}
