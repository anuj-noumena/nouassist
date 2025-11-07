using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.ProjectManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ProjectsDomainTestBase<TStartupModule> : ProjectsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
