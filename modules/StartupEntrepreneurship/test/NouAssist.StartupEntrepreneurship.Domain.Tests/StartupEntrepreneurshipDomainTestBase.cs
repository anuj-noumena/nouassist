using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class StartupEntrepreneurshipDomainTestBase<TStartupModule> : StartupEntrepreneurshipTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
