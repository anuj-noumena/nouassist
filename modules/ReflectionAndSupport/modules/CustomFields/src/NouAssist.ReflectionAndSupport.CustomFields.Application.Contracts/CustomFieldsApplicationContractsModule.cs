using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(CustomFieldsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CustomFieldsApplicationContractsModule : AbpModule
{

}
