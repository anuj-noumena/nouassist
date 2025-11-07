using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.ContactManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ContactManagementDomainTestBase<TStartupModule> : ContactManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
