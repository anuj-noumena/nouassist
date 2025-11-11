using Volo.Abp.Modularity;

namespace NouAssist.LearningDevelopment;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class LearningDevelopmentDomainTestBase<TStartupModule> : LearningDevelopmentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
