using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class GoalsApplicationTestBase<TStartupModule> : GoalsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
