using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class DiscussionThreadsApplicationTestBase<TStartupModule> : DiscussionThreadsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
