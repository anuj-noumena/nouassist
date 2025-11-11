using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Customization;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CustomizationDomainSharedModule)
)]
public class CustomizationDomainModule : AbpModule
{

}
