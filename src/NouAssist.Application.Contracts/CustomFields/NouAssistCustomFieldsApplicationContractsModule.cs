using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace NouAssist.CustomFields
{
    [DependsOn(
        typeof(NouAssistCustomFieldsDomainSharedModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule)
    )]
    public class NouAssistCustomFieldsApplicationContractsModule : AbpModule
    {
    }
}
