using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class GoalTrackingDomainTestBase<TStartupModule> : GoalTrackingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
