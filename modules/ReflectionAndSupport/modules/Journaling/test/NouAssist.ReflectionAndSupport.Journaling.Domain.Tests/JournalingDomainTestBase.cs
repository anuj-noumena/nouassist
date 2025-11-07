using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.Journaling;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class JournalingDomainTestBase<TStartupModule> : JournalingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
