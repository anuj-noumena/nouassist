using Volo.Abp.Modularity;

namespace NouAssist.Search;

[DependsOn(
    typeof(SearchApplicationModule),
    typeof(SearchDomainTestModule)
    )]
public class SearchApplicationTestModule : AbpModule
{

}
