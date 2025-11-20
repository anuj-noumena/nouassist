using System.Threading.Tasks;
using Volo.Abp.Modularity;
using Xunit;

namespace NouAssist.Integrations.Samples;

public abstract class SampleManager_Tests<TStartupModule> : IntegrationsDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    //private readonly SampleManager _sampleManager;

    public SampleManager_Tests()
    {
        //_sampleManager = GetRequiredService<SampleManager>();
    }

    [Fact]
    public Task Method1Async()
    {
        return Task.CompletedTask;
    }
}
