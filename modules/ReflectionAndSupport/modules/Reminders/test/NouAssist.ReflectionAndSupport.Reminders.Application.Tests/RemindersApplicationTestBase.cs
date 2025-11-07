using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reminders;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class RemindersApplicationTestBase<TStartupModule> : RemindersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
