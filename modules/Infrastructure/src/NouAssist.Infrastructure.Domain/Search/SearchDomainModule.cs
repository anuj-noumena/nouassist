using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Search;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(SearchDomainSharedModule)
)]
public class SearchDomainModule : AbpModule
{

}
