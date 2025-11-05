using Volo.Abp.Modularity;

namespace NouAssist.Search;

[DependsOn(
    typeof(SearchDomainModule),
    typeof(SearchTestBaseModule)
)]
public class SearchDomainTestModule : AbpModule
{

}
