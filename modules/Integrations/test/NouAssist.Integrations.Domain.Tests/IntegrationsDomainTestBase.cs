using Volo.Abp.Modularity;

namespace NouAssist.Integrations;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class IntegrationsDomainTestBase<TStartupModule> : IntegrationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
