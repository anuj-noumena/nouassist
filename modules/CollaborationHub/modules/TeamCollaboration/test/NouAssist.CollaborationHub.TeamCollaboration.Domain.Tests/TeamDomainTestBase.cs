using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class TeamDomainTestBase<TStartupModule> : TeamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
