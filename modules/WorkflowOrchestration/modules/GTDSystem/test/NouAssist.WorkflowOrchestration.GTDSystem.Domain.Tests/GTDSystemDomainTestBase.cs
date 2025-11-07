using Volo.Abp.Modularity;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class GTDSystemDomainTestBase<TStartupModule> : GTDSystemTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
