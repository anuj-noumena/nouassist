using Volo.Abp.Modularity;

namespace NouAssist.Meetings;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class MeetingsDomainTestBase<TStartupModule> : MeetingsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
