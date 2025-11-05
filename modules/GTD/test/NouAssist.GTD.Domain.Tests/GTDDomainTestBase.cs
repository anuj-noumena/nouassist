using Volo.Abp.Modularity;

namespace NouAssist.GTD;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class GTDDomainTestBase<TStartupModule> : GTDTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
