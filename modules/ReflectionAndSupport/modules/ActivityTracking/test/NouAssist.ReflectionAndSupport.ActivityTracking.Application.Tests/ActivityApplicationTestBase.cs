using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ActivityApplicationTestBase<TStartupModule> : ActivityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
