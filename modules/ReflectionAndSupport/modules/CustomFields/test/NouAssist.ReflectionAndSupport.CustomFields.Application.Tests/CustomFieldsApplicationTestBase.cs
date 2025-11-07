using Volo.Abp.Modularity;

namespace NouAssist.ReflectionAndSupport.CustomFields;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class CustomFieldsApplicationTestBase<TStartupModule> : CustomFieldsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
