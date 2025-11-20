using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(BusinessOperationsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class BusinessOperationsApplicationContractsModule : AbpModule
{

}
