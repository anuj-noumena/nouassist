using Volo.Abp.Modularity;

namespace NouAssist.Habits;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class HabitsApplicationTestBase<TStartupModule> : HabitsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
