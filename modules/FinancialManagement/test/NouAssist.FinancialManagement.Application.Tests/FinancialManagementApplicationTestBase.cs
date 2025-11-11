using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class FinancialManagementApplicationTestBase<TStartupModule> : FinancialManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
