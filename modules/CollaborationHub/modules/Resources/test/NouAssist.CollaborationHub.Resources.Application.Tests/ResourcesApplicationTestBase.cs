using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ResourcesApplicationTestBase<TStartupModule> : ResourcesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
