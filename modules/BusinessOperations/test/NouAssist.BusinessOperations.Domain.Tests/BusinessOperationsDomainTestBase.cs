using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class BusinessOperationsDomainTestBase<TStartupModule> : BusinessOperationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
