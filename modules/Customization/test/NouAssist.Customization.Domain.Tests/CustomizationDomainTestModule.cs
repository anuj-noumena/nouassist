using Volo.Abp.Modularity;

namespace NouAssist.Customization;

[DependsOn(
    typeof(CustomizationDomainModule),
    typeof(CustomizationTestBaseModule)
)]
public class CustomizationDomainTestModule : AbpModule
{

}
