using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class WorkManagementDomainTestBase<TStartupModule> : WorkManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
