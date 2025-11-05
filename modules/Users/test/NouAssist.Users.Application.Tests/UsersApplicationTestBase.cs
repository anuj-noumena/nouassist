using Volo.Abp.Modularity;

namespace NouAssist.Users;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class UsersApplicationTestBase<TStartupModule> : UsersTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
