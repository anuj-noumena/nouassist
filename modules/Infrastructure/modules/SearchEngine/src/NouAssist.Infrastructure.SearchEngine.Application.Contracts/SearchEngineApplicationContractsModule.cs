using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(SearchEngineDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class SearchEngineApplicationContractsModule : AbpModule
{

}
