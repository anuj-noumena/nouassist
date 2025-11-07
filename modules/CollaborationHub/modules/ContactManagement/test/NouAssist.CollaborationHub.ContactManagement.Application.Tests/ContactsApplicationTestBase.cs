using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ContactsApplicationTestBase<TStartupModule> : ContactsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
