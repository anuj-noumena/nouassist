using Volo.Abp.Modularity;

namespace NouAssist.Customization;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CustomizationDomainTestBase<TStartupModule> : CustomizationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
