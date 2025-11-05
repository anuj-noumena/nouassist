using Volo.Abp.Modularity;

namespace NouAssist.Habits;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class HabitsDomainTestBase<TStartupModule> : HabitsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
