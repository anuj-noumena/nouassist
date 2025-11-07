using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Reviews;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ReviewsDomainTestBase<TStartupModule> : ReviewsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
