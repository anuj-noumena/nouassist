using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CollaborationHubDomainTestBase<TStartupModule> : CollaborationHubTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
