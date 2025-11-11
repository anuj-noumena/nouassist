using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class LearningDevelopmentApplicationTestBase<TStartupModule> : LearningDevelopmentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
