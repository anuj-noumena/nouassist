using Volo.Abp.Modularity;

namespace NouAssist.CustomFields;

[DependsOn(
    typeof(CustomFieldsApplicationModule),
    typeof(CustomFieldsDomainTestModule)
    )]
public class CustomFieldsApplicationTestModule : AbpModule
{

}
