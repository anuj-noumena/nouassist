using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessDomainModule),
    typeof(IdentityAccessApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpMapperlyModule)
    )]
public class IdentityAccessApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper<IdentityAccessApplicationModule>();
    }
}
