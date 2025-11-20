using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class StartupEntrepreneurshipApplicationTestBase<TStartupModule> : StartupEntrepreneurshipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
