using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ApprovalWorkflowsApplicationTestBase<TStartupModule> : ApprovalWorkflowsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
