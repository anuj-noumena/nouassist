using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CustomFieldsDomainSharedModule)
)]
public class CustomFieldsDomainModule : AbpModule
{

}
