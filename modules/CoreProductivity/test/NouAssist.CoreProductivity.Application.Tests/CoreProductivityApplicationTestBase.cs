using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CoreProductivityApplicationTestBase<TStartupModule> : CoreProductivityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
