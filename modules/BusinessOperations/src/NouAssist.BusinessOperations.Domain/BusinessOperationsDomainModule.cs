using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(BusinessOperationsDomainSharedModule)
)]
public class BusinessOperationsDomainModule : AbpModule
{

}
