using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class JournalingApplicationTestBase<TStartupModule> : JournalingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
