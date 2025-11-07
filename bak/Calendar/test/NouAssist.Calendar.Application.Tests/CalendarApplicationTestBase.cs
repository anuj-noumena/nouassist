using Volo.Abp.Modularity;

namespace NouAssist.Calendar;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CalendarApplicationTestBase<TStartupModule> : CalendarTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
