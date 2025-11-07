using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ReflectionAndSupportApplicationTestBase<TStartupModule> : ReflectionAndSupportTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
