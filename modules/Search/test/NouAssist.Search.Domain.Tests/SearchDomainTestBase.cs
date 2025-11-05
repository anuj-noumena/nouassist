using Volo.Abp.Modularity;

namespace NouAssist.Search;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class SearchDomainTestBase<TStartupModule> : SearchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
