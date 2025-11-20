using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class RequirementsManagementDomainTestBase<TStartupModule> : RequirementsManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
