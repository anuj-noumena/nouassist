using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class TasksApplicationTestBase<TStartupModule> : TasksTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
