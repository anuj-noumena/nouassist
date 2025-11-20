using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class RequirementsManagementApplicationTestBase<TStartupModule> : RequirementsManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
