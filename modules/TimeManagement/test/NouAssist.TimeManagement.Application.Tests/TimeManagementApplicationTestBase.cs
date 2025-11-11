using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class TimeManagementApplicationTestBase<TStartupModule> : TimeManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
