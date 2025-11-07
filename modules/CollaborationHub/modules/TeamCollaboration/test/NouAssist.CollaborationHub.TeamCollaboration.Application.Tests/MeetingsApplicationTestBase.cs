using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.TeamCollaboration;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class MeetingsApplicationTestBase<TStartupModule> : MeetingsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
