using Volo.Abp.Modularity;

namespace NouAssist.Reminders;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class RemindersDomainTestBase<TStartupModule> : RemindersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
