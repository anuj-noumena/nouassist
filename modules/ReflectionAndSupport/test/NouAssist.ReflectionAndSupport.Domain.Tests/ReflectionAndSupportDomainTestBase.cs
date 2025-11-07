using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ReflectionAndSupportDomainTestBase<TStartupModule> : ReflectionAndSupportTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
