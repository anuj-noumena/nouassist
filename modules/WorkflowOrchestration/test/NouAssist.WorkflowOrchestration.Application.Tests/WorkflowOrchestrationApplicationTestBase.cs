using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class WorkflowOrchestrationApplicationTestBase<TStartupModule> : WorkflowOrchestrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
