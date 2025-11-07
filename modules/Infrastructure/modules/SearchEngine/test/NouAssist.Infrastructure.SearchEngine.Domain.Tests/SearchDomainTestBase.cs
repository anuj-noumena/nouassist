using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure.SearchEngine;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class SearchDomainTestBase<TStartupModule> : SearchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
