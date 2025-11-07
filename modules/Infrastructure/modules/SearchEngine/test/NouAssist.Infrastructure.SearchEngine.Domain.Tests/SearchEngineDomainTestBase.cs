using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class SearchEngineDomainTestBase<TStartupModule> : SearchEngineTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
