using Volo.Abp.Modularity;

namespace NouAssist.Notification;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class NotificationDomainTestBase<TStartupModule> : NotificationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
