using NouAssist.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace NouAssist.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(NouAssistEntityFrameworkCoreModule),
    typeof(NouAssistApplicationContractsModule)
)]
public class NouAssistDbMigratorModule : AbpModule
{
}
