using Volo.Abp.Modularity;

namespace NouAssist.Customization;

[DependsOn(
    typeof(CustomizationApplicationModule),
    typeof(CustomizationDomainTestModule)
    )]
public class CustomizationApplicationTestModule : AbpModule
{

}
