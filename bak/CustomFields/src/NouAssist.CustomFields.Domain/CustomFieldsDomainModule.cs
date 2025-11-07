using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CustomFields;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CustomFieldsDomainSharedModule)
)]
public class CustomFieldsDomainModule : AbpModule
{

}
