using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class GoalsDomainTestBase<TStartupModule> : GoalsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
