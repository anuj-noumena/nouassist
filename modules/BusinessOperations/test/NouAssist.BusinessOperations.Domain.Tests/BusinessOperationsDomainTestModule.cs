using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(BusinessOperationsDomainModule),
    typeof(BusinessOperationsTestBaseModule)
)]
public class BusinessOperationsDomainTestModule : AbpModule
{

}
