using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class IdentityAccessApplicationTestBase<TStartupModule> : IdentityAccessTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
