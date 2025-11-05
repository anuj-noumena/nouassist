using Volo.Abp.Modularity;

namespace NouAssist.Notifications;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class NotificationsApplicationTestBase<TStartupModule> : NotificationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
