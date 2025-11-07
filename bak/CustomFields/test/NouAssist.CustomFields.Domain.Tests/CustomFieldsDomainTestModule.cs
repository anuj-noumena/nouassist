using Volo.Abp.Modularity;

namespace NouAssist.CustomFields;

[DependsOn(
    typeof(CustomFieldsDomainModule),
    typeof(CustomFieldsTestBaseModule)
)]
public class CustomFieldsDomainTestModule : AbpModule
{

}
