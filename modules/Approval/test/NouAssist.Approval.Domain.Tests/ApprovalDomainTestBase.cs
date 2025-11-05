using Volo.Abp.Modularity;

namespace NouAssist.Approval;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class ApprovalDomainTestBase<TStartupModule> : ApprovalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
