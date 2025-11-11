using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(FinancialManagementDomainSharedModule)
)]
public class FinancialManagementDomainModule : AbpModule
{

}
