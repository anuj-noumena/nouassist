using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ProjectManagementDomainTestBase<TStartupModule> : ProjectManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
