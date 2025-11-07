using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.Notifications;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class NotificationsDomainTestBase<TStartupModule> : NotificationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
