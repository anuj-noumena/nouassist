using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class WorkflowApplicationTestBase<TStartupModule> : WorkflowTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
