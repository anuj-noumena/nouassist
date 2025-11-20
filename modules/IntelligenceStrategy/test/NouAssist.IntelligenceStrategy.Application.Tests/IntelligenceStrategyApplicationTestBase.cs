using Volo.Abp.Modularity;

namespace NouAssist.IntelligenceStrategy;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class IntelligenceStrategyApplicationTestBase<TStartupModule> : IntelligenceStrategyTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
