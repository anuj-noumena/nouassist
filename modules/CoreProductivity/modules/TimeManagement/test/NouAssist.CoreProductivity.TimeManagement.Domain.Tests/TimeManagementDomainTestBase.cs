using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TimeManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class TimeManagementDomainTestBase<TStartupModule> : TimeManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
