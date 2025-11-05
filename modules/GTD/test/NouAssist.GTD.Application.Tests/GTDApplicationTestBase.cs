using Volo.Abp.Modularity;

namespace NouAssist.GTD;

/* Inherit from this class for your application layer tests.
 * See SampleAppService_Tests for example.
 */
public abstract class GTDApplicationTestBase<TStartupModule> : GTDTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
