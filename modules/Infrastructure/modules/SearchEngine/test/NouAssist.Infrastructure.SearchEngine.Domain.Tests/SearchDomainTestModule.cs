using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(SearchEngineDomainModule),
    typeof(SearchEngineTestBaseModule)
)]
public class SearchEngineDomainTestModule : AbpModule
{

}
