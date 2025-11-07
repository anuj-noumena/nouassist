using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class GTDSystemApplicationTestBase<TStartupModule> : GTDSystemTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
