using Volo.Abp.Modularity;

namespace NouAssist.Approval;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class ApprovalApplicationTestBase<TStartupModule> : ApprovalTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
