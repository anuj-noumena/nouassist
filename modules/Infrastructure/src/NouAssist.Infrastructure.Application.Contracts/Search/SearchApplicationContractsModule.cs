using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Search;

[DependsOn(
    typeof(SearchDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SearchApplicationContractsModule : AbpModule
{

}
