using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class IdentityAccessDomainTestBase<TStartupModule> : IdentityAccessTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
