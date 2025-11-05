using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel;

/* Inherit from this class for your domain layer tests.
 * See SampleManager_Tests for example.
 */
public abstract class SharedKernelDomainTestBase<TStartupModule> : SharedKernelTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
