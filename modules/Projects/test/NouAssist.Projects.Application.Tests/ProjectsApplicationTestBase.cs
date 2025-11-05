using Volo.Abp.Modularity;

namespace NouAssist.Projects;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ProjectsApplicationTestBase<TStartupModule> : ProjectsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
