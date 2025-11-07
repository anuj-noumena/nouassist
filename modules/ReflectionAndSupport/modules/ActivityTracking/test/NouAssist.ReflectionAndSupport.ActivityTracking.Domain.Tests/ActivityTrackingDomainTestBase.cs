using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.ActivityTracking;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ActivityTrackingDomainTestBase<TStartupModule> : ActivityTrackingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
