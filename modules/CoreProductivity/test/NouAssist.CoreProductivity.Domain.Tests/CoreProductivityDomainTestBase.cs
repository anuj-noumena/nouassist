using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CoreProductivityDomainTestBase<TStartupModule> : CoreProductivityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
