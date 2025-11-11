using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class AssetManagementApplicationTestBase<TStartupModule> : AssetManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
