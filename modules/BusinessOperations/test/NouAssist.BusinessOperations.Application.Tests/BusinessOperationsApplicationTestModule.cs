using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations;

[DependsOn(
    typeof(BusinessOperationsApplicationModule),
    typeof(BusinessOperationsDomainTestModule)
    )]
public class BusinessOperationsApplicationTestModule : AbpModule
{

}
