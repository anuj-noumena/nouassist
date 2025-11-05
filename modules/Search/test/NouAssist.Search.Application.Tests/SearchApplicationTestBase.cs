using Volo.Abp.Modularity;

namespace NouAssist.Search;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class SearchApplicationTestBase<TStartupModule> : SearchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
