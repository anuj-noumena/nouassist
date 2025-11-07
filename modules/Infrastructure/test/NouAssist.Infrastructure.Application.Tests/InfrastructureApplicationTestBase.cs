using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class InfrastructureApplicationTestBase<TStartupModule> : InfrastructureTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
