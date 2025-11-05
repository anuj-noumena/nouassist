using Volo.Abp.Modularity;

namespace NouAssist.Tasks;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class TasksDomainTestBase<TStartupModule> : TasksTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
