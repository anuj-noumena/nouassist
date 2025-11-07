using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class WorkflowEngineDomainTestBase<TStartupModule> : WorkflowEngineTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
