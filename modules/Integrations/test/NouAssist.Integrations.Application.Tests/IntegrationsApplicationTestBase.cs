using Volo.Abp.Modularity;

namespace NouAssist.Integrations;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class IntegrationsApplicationTestBase<TStartupModule> : IntegrationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
