using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Customization.CustomFields;

[DependsOn(
    typeof(CustomFieldsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CustomFieldsApplicationContractsModule : AbpModule
{

}
