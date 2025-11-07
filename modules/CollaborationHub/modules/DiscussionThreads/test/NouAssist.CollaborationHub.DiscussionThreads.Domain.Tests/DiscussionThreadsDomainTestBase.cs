using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class DiscussionThreadsDomainTestBase<TStartupModule> : DiscussionThreadsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
