using Volo.Abp.Modularity;

namespace NouAssist.Contacts;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ContactsDomainTestBase<TStartupModule> : ContactsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
