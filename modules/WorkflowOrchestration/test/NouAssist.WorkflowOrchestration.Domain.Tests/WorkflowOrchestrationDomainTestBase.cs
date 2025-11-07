using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class WorkflowOrchestrationDomainTestBase<TStartupModule> : WorkflowOrchestrationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
