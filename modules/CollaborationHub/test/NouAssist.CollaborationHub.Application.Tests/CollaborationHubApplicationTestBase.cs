using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CollaborationHubApplicationTestBase<TStartupModule> : CollaborationHubTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
