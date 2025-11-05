using Volo.Abp.Modularity;

namespace NouAssist.Team;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class TeamApplicationTestBase<TStartupModule> : TeamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
