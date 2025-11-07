using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(SearchEngineApplicationModule),
    typeof(SearchEngineDomainTestModule)
    )]
public class SearchEngineApplicationTestModule : AbpModule
{

}
