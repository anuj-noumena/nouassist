using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class FinancialManagementApplicationContractsModule : AbpModule
{

}
