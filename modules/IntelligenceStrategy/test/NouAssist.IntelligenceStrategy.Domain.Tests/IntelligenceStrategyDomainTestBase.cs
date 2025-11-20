using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class IntelligenceStrategyDomainTestBase<TStartupModule> : IntelligenceStrategyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
