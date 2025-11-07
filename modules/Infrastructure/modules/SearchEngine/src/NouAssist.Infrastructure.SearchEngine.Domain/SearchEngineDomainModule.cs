using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(SearchEngineDomainSharedModule)
)]
public class SearchEngineDomainModule : AbpModule
{

}
