using Volo.Abp.Modularity;

namespace NouAssist.Projects;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ProjectsDomainTestBase<TStartupModule> : ProjectsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
