using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.Resources;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ResourcesDomainTestBase<TStartupModule> : ResourcesTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
