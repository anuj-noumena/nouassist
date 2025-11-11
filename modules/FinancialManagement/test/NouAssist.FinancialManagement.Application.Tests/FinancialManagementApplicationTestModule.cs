using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(FinancialManagementApplicationModule),
    typeof(FinancialManagementDomainTestModule)
    )]
public class FinancialManagementApplicationTestModule : AbpModule
{

}
