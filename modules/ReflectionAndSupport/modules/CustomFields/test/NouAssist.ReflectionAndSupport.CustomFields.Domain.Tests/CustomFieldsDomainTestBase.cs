using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class CustomFieldsDomainTestBase<TStartupModule> : CustomFieldsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
