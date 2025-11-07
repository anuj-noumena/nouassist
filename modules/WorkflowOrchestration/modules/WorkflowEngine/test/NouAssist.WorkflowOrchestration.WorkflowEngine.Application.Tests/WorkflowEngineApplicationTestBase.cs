using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class WorkflowEngineApplicationTestBase<TStartupModule> : WorkflowEngineTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
