using Volo.Abp.Modularity;

namespace NouAssist.AssetManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class AssetManagementDomainTestBase<TStartupModule> : AssetManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
