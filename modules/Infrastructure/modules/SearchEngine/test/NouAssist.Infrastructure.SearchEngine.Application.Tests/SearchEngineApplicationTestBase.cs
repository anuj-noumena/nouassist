using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class SearchEngineApplicationTestBase<TStartupModule> : SearchEngineTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
