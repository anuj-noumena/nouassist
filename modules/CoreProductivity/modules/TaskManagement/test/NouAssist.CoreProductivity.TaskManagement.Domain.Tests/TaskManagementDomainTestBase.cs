using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TaskManagement;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class TaskManagementDomainTestBase<TStartupModule> : TaskManagementTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
