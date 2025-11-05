using Volo.Abp.Modularity;

namespace NouAssist.Reviews;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ReviewsApplicationTestBase<TStartupModule> : ReviewsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
