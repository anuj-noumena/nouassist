using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(CustomFieldsDomainModule),
    typeof(CustomFieldsTestBaseModule)
)]
public class CustomFieldsDomainTestModule : AbpModule
{

}
