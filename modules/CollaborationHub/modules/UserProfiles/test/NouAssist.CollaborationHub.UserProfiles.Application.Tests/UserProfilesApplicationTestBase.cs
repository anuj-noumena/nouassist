using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class UserProfilesApplicationTestBase<TStartupModule> : UserProfilesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
