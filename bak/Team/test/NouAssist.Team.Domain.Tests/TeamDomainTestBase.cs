using Volo.Abp.Modularity;

namespace NouAssist.Team;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class TeamDomainTestBase<TStartupModule> : TeamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
