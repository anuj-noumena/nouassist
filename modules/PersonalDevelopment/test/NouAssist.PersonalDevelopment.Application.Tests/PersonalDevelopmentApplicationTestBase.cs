using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class PersonalDevelopmentApplicationTestBase<TStartupModule> : PersonalDevelopmentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
