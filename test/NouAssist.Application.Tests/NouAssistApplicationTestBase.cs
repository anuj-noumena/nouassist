using Volo.Abp.Modularity;

namespace NouAssist;

public abstract class NouAssistApplicationTestBase<TStartupModule> : NouAssistTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
