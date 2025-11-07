using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ApprovalWorkflowsDomainTestBase<TStartupModule> : ApprovalWorkflowsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
