using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(PersonalDevelopmentDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class PersonalDevelopmentApplicationContractsModule : AbpModule
{

}
