using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields;

[DependsOn(
    typeof(CustomFieldsApplicationModule),
    typeof(CustomFieldsDomainTestModule)
    )]
public class CustomFieldsApplicationTestModule : AbpModule
{

}
