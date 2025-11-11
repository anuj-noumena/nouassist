using Volo.Abp.Modularity;

namespace NouAssist.Customization;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CustomizationApplicationTestBase<TStartupModule> : CustomizationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
