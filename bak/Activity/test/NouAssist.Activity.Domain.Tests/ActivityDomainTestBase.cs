using Volo.Abp.Modularity;

namespace NouAssist.Activity;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ActivityDomainTestBase<TStartupModule> : ActivityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
