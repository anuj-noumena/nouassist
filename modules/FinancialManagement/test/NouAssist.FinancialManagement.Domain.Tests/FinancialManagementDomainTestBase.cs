using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class FinancialManagementDomainTestBase<TStartupModule> : FinancialManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
