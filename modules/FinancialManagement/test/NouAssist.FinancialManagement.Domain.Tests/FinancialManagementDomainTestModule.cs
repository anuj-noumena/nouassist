using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementDomainModule),
    typeof(FinancialManagementTestBaseModule)
)]
public class FinancialManagementDomainTestModule : AbpModule
{

}
