using Volo.Abp.Modularity;

namespace NouAssist;

/* Inherit from this class for your domain layer tests. */
public abstract class NouAssistDomainTestBase<TStartupModule> : NouAssistTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
