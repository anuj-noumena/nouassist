using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class InfrastructureDomainTestBase<TStartupModule> : InfrastructureTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
