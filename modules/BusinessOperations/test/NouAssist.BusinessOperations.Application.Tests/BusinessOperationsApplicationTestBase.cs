using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class BusinessOperationsApplicationTestBase<TStartupModule> : BusinessOperationsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
