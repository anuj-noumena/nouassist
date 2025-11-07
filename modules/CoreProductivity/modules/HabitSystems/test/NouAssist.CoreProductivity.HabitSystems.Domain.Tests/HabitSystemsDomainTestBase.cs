using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class HabitSystemsDomainTestBase<TStartupModule> : HabitSystemsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
