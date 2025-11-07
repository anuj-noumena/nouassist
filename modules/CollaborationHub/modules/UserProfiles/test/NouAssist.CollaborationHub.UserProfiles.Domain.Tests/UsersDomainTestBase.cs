using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class UsersDomainTestBase<TStartupModule> : UsersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
