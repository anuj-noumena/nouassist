using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class WorkManagementApplicationTestBase<TStartupModule> : WorkManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
