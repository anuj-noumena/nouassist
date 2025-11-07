using Volo.Abp.Modularity;

namespace NouAssist.Comments;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CommentsApplicationTestBase<TStartupModule> : CommentsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
